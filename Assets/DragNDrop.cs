using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragNDrop : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private GameObject rightMenu;
    private Vector3 menuOffest;
    private Ray ray;
    private RaycastHit hit;
    private GameObject rightCanvas;
    private Vector3 mouseStartPos;
    private Vector3 mouseEndPos;
    private float mouseStartTime;
    private float mouseEndTime;
    public bool canBeRightClicked = true;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mouseStartTime = Time.time;
        // Store offset = gameobject world pos - mouse world pos
        // offset allows you to grab the object from the side of the circle, not just the center
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        rightCanvas.GetComponent<RightClickHelper>().HideRightMenu();
        //starting position for flicking sphere to increase velocity
        Vector3 mousePos = Input.mousePosition * -1;
        mousePos.z = 0;
        mouseStartPos = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    
    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
		gameObject.GetComponent<Rigidbody>().MovePosition(GetMouseAsWorldPoint() + mOffset);
    }

    void OnMouseUp()
    {
        //get release mouse position and time
        Vector3 mousePos = Input.mousePosition * -1.0f;
        mousePos.z = 0;
        mouseEndTime = Time.time;
        float timePassed = (mouseEndTime -mouseStartTime);
        //Debug.Log(timePassed);

        //convert mouse to world position
        mouseEndPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseEndPos.z = 0;

        //Debug.Log("Min" + Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMin)).y);
        //Debug.Log(Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMax)).y);
        //makes sure the gameobject is inside the camera
     

        //Debug.Log(-Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMin, 0)).x);

        //logic to determine flick vs drag && calculating force vector to be added
        //Debug.Log(Vector3.Distance(mouseStartPos, mouseEndPos));
        if (Vector3.Distance(mouseStartPos, mouseEndPos) > 12 && timePassed < 0.3f && Time.timeScale != 0 && GameObject.Find("GameObject").GetComponent<main>().useFlick)
        {
            //makes sure flick is in the screen and corrects it
            if (gameObject.transform.position.y > -Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMin)).y)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, -Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMin)).y - 2, 0);
                gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position);
                mousePos.y = gameObject.transform.position.y;
                //Debug.Log("up");
            }
            //if it is below the camera
            if (gameObject.transform.position.y < -Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMax)).y)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, -Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMax)).y + 2, 0);
                gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position);
                mousePos.y = gameObject.transform.position.y;
                //Debug.Log("below");
            }

            //if it is to far to the right of the camera 
            if (gameObject.transform.position.x > -Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMin, 0)).x)
            {
                gameObject.transform.position = new Vector3(-Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMin, 0)).x - 2, gameObject.transform.position.y, 0);
                gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position);
                mousePos.x = gameObject.transform.position.x;
                //Debug.Log("right");
            }
            //if it is to far to the left of the camera
            if (gameObject.transform.position.x < -Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMax, 0)).x)
            {
                gameObject.transform.position = new Vector3(-Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMax, 0)).x + 2, gameObject.transform.position.y, 0);
                gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position);
                mousePos.x = gameObject.transform.position.x;
                //Debug.Log("left");
            }

            //calculates and flicks the particle
            Vector3 throwDir = (mouseStartPos - mouseEndPos).normalized;
            Vector3 forceToAdd = (.5f * throwDir * (mouseStartPos - mouseEndPos).sqrMagnitude);
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(forceToAdd.x, forceToAdd.y, 0);
            //Debug.Log(throwDir * (mouseStartPos - mouseEndPos).sqrMagnitude);
        }
        else
        {
            //makes sure flick is in the screen and corrects it
            if (gameObject.transform.position.y > -Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMin)).y)
            {
                DestroyParticle();
                //Debug.Log("up");
            }
            //if it is below the camera
            else if (gameObject.transform.position.y < -Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMax)).y)
            {
                DestroyParticle();

                //Debug.Log("below");
            }

            //if it is to far to the right of the camera 
            else if (gameObject.transform.position.x > -Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMin, 0)).x)
            {
                DestroyParticle();

                //Debug.Log("right");
            }
            //if it is to far to the left of the buffet table
            else if (gameObject.transform.position.x < GameObject.Find("left wall").transform.position.x)
            {
                DestroyParticle();
                //Debug.Log("left");
            }
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); 
        }
    }

    private void DestroyParticle()
    {
        if (gameObject.name.Contains("[P] Water"))
        {
            //hydrogen one
            gameObject.transform.GetChild(0).name = "destroyed";
            GameObject.Find("GameObject").GetComponent<forces>().gameObjects.Remove(gameObject.transform.GetChild(0).gameObject);
            Destroy(gameObject.transform.GetChild(0).gameObject);
            //hydrogen two
            gameObject.transform.GetChild(1).name = "destroyed";
            GameObject.Find("GameObject").GetComponent<forces>().gameObjects.Remove(gameObject.transform.GetChild(1).gameObject);
            Destroy(gameObject.transform.GetChild(1).gameObject);
            //oxygen
            gameObject.name = "destroyed";
            GameObject.Find("GameObject").GetComponent<forces>().gameObjects.Remove(gameObject);
            GameObject.Find("GameObject").GetComponent<forces>().nonObjects.Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
            gameObject.name = "destroyed";
            GameObject.Find("GameObject").GetComponent<forces>().gameObjects.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position);
        //defining objects/variables
        rightCanvas = GameObject.Find("Right-Click Canvas");
        rightMenu = rightCanvas.GetComponent<RightClickHelper>().rightMenu;
        rightMenu.SetActive(false);
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    }

    //updates every frame
    void Update()
    {
        //set up for finding the object that was right clicked
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //like OnMouseDown, but for right-click
        if (Input.GetMouseButtonDown(1))
        {
            //find out if an object was hit 
            if (Physics.Raycast(ray, out hit) && canBeRightClicked)
            {

                //calculates where the menu must be
                rightCanvas.GetComponent<RightClickHelper>().CheckRightVisablity();
        
                //------------------------sets the position of the right-click menu------------------------
                //rightMenu.transform.position = Input.mousePosition + menuOffest;
                //makes sure that right click menu is in screen
                

                //makes menu visible
                rightMenu.SetActive(true);

                //update the variable so that RightClickHelper knows what sphere to edit
                rightCanvas.GetComponent<RightClickHelper>().currentSphere = hit.rigidbody.gameObject;

                //------------------------updates menu values on click------------------------

                //mass
                rightCanvas.GetComponent<RightClickHelper>().Mass.GetComponent<InputField>().text = hit.rigidbody.mass.ToString();

                //charge
                rightCanvas.GetComponent<RightClickHelper>().Charge.GetComponent<InputField>().text = hit.rigidbody.gameObject.GetComponent<charger>().charge.ToString();

                //color
                if (hit.rigidbody.gameObject.GetComponent<Renderer>().material.color == Color.red)
                {
                    rightCanvas.GetComponent<RightClickHelper>().Color1.GetComponent<Toggle>().isOn = true;
                    rightCanvas.GetComponent<RightClickHelper>().toggleGroup.GetComponent<ToggleGroup>().NotifyToggleOn(rightCanvas.GetComponent<RightClickHelper>().Color1.GetComponent<Toggle>());
                }else
                if (hit.rigidbody.gameObject.GetComponent<Renderer>().material.color == Color.blue)
                {
                    rightCanvas.GetComponent<RightClickHelper>().Color2.GetComponent<Toggle>().isOn = true;
                    rightCanvas.GetComponent<RightClickHelper>().toggleGroup.GetComponent<ToggleGroup>().NotifyToggleOn(rightCanvas.GetComponent<RightClickHelper>().Color2.GetComponent<Toggle>());
                }else
                if (hit.rigidbody.gameObject.GetComponent<Renderer>().material.color == Color.green)
                {
                    rightCanvas.GetComponent<RightClickHelper>().Color3.GetComponent<Toggle>().isOn = true;
                    rightCanvas.GetComponent<RightClickHelper>().toggleGroup.GetComponent<ToggleGroup>().NotifyToggleOn(rightCanvas.GetComponent<RightClickHelper>().Color3.GetComponent<Toggle>());
                }else
                if (hit.rigidbody.gameObject.GetComponent<Renderer>().material.color == Color.yellow)
                {
                    rightCanvas.GetComponent<RightClickHelper>().Color4.GetComponent<Toggle>().isOn = true;
                    rightCanvas.GetComponent<RightClickHelper>().toggleGroup.GetComponent<ToggleGroup>().NotifyToggleOn(rightCanvas.GetComponent<RightClickHelper>().Color4.GetComponent<Toggle>());
                }

                //size
                if (hit.rigidbody.gameObject.transform.localScale == new Vector3(1, 1, 1))
                {
                    rightCanvas.GetComponent<RightClickHelper>().Size1.GetComponent<Toggle>().isOn = true;
                }
                else
                {
                    rightCanvas.GetComponent<RightClickHelper>().Size2.GetComponent<Toggle>().isOn = true;
                }
                
            }
        }
        
      
    }
    private bool CheckConstraints(Rigidbody rb)
    {
        bool itWorks;
        if (rb.constraints == RigidbodyConstraints.FreezeAll)
        {
            itWorks = true;
        }
        else
        {
            itWorks = false;
        }
        return itWorks;
    }
}
                //else if (hit.rigidbody.gameObject.transform.localScale == new Vector3(2, 2, 2)

                    //rightCanvas.GetComponent<RightClickHelper>().Size2.GetComponent<Toggle>().isOn = true;
                
            //rightCanvas.GetComponent<RightClickHelper>().Elastic.GetComponent<Toggle>().isOn = hit.rigidbody.gameObject.GetComponent<elastic>().enabled;

        
    //rightCanvas.GetComponent<RightClickHelper>().currentSphere = hit.rigidbody.gameObject;

                //anchor   -this made the size bool act weird when setting the bool to is on- 
                //bool test;
                //test = CheckConstraints(hit.rigidbody.gameObject.GetComponent<Rigidbody>());
                //rightCanvas.GetComponent<RightClickHelper>().anchorToggle.GetComponent<Toggle>().isOn = test;
                    /*CheckConstraints(hit.rigidbody.gameObject.GetComponent<Rigidbody>())*/
                //Debug.Log(test);


                //UpdateRightMenuStats();
                //Debug.Log("yep");



                /*
                if (Input.GetMouseButtonDown(0))
                {
                    //rightMenu.SetActive(false);
                }

                //to make sure that only one object is being effective
                if(gameObject.GetComponent<SphereCollider>().bounds.Contains(rightCanvas.GetComponent<RightClickHelper>().triggerPoint.transform.position))
                {
                    rightCanvas.GetComponent<RightClickHelper>().currentSphere = gameObject;
                   //Debug.Log("target acquired: " + rightCanvas.GetComponent<RightClickHelper>().currentSphere);
                }*/


                /*
        //private float num;
        private string tempText;
        private float tempMass;
        private void UpdateRightMenuStats()
        {

            //updating Mass
            rightCanvas.GetComponent<RightClickHelper>().Mass.GetComponent<InputField>().text = tempText;
            hit.rigidbody.mass = tempMass;
            tempText = tempMass.ToString();


            tempText = tempMass.ToString();
               //Debug.Log(tempText);

                StringToFloat(rightCanvas.GetComponent<RightClickHelper>().Mass.GetComponent<InputField>().text, num );
                num = 3;
                //num = gameObject.GetComponent<Rigidbody>().mass;
        }

        public void StringToFloat(string inputString, float floatToRetrun)
        {
            if (inputString != null)
            {
                 floatToRetrun = float.Parse(inputString);
            }
        }
                */
     
