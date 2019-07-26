using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drag : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private GameObject rightMenu;
    private Vector3 menuOffest;
    private Ray ray;
    private RaycastHit hit;
    private GameObject rightCanvas;
    private GameObject shpereBeingRightCkicked;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        // offset allows you to grab the object from the side of the circle, not just the center
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        rightCanvas.GetComponent<RightClickHelper>().HideRightMenu();
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
    
    //the tempary varible 
    private float tempMass;
    private float tempDrag;
    private float tempAngularDrag;
    private bool tempUseGravity;
    private bool tempIsKinematic;
    private RigidbodyInterpolation tempInterpolation;
    private CollisionDetectionMode tempCollisionDetectionMode;
    private RigidbodyConstraints tempConstraints;
    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
        //stores the values of the rigidbody and then destorys it
        if(gameObject.GetComponent<Rigidbody>() != null)
        {
            tempMass = gameObject.GetComponent<Rigidbody>().mass;
            tempDrag = gameObject.GetComponent<Rigidbody>().drag;
            tempAngularDrag = gameObject.GetComponent<Rigidbody>().angularDrag;
            tempUseGravity = gameObject.GetComponent<Rigidbody>().useGravity;
            tempIsKinematic = gameObject.GetComponent<Rigidbody>().isKinematic;
            tempInterpolation = gameObject.GetComponent<Rigidbody>().interpolation;
            tempCollisionDetectionMode = gameObject.GetComponent<Rigidbody>().collisionDetectionMode;
            tempConstraints = gameObject.GetComponent<Rigidbody>().constraints;
            //elastic gave tons of erros while the rigidbody was disabled
            gameObject.GetComponent<elastic>().enabled = (false);
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
        //Debug.Log(this.transform.position);
    }
   
    void OnMouseUp()
    {
        //restore rigidbody and varibles 
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().mass = tempMass;
        gameObject.GetComponent<Rigidbody>().drag = tempDrag;
        gameObject.GetComponent<Rigidbody>().angularDrag = tempAngularDrag;
        gameObject.GetComponent<Rigidbody>().useGravity = tempUseGravity;
        gameObject.GetComponent<Rigidbody>().isKinematic = tempIsKinematic;
        gameObject.GetComponent<Rigidbody>().interpolation = tempInterpolation;
        gameObject.GetComponent<Rigidbody>().collisionDetectionMode = tempCollisionDetectionMode;
        gameObject.GetComponent<Rigidbody>().constraints = tempConstraints;
        gameObject.GetComponent<elastic>().enabled = (true);
        //this was done to fix a bug where spheres could only be moved once while the simulation was paused
    }

    void Start()
    {
        //defining objects/varibles
        rightCanvas = GameObject.Find("Right-Click Canvas");
        rightMenu = rightCanvas.GetComponent<RightClickHelper>().rightMenu;
        rightMenu.SetActive(false);
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z; //     |
        //to do, figure out how to modify this when the image is off screen with -/+      V
        menuOffest = new Vector3(rightMenu.GetComponent<RectTransform>().rect.width / 2f, -rightMenu.GetComponent<RectTransform>().rect.height / 2f, mZCoord);
    }

    //updates everyframe
    void Update()
    {
        //set up for finding the object that was right clicked
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //like OnMouseDown, but for right-click
        if (Input.GetMouseButtonDown(1))
        {
            //find out if an object was hit 
            if (Physics.Raycast(ray, out hit))
            {
                //------------------------sets the position of the right-click menu------------------------
                rightMenu.transform.position = Input.mousePosition + menuOffest;
                //makes sure that right click menu is in screen
                /*if (rightCanvas.GetComponent<Rect>().Contains(rightCanvas.GetComponent<RightClickHelper>().triggerPoint.transform.position))
                {
                    // Inside
                    Debug.Log("Inside");
                }*/

                //makes menu visable
                rightMenu.SetActive(true);

                //update the varible so that RightClickHelper knows what sphere to edit
                rightCanvas.GetComponent<RightClickHelper>().currentSphere = hit.rigidbody.gameObject;

                //------------------------updates menu vaules on click------------------------

                //mass
                rightCanvas.GetComponent<RightClickHelper>().Mass.GetComponent<InputField>().text = hit.rigidbody.mass.ToString();

                //charge
                rightCanvas.GetComponent<RightClickHelper>().Charge.GetComponent<InputField>().text = hit.rigidbody.gameObject.GetComponent<charger>().charge.ToString();
                
                //Temputure
                //nothing to update
                
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
}
                //else if (hit.rigidbody.gameObject.transform.localScale == new Vector3(2, 2, 2)

                    //rightCanvas.GetComponent<RightClickHelper>().Size2.GetComponent<Toggle>().isOn = true;
                
            //rightCanvas.GetComponent<RightClickHelper>().Elastic.GetComponent<Toggle>().isOn = hit.rigidbody.gameObject.GetComponent<elastic>().enabled;

        
    //rightCanvas.GetComponent<RightClickHelper>().currentSphere = hit.rigidbody.gameObject;

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
                    Debug.Log("target aquired: " + rightCanvas.GetComponent<RightClickHelper>().currentSphere);
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
                Debug.Log(tempText);

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
     
