using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightClickHelper : MonoBehaviour
{
    //------------------------all the varibles------------------------
    public GameObject rightMenu;
    public GameObject Mass;
    public GameObject Charge;
    public GameObject anchorButton;
    public GameObject toggleGroup;
    public GameObject Color1;
    public GameObject Color2;
    public GameObject Color3;
    public GameObject Color4;
    public GameObject Size1;
    public GameObject Size2;
    public RigidbodyConstraints AnchorConstraints;
    public RigidbodyConstraints UnAnchorConstraints;
    public GameObject triggerPoint1;
    public GameObject triggerPoint2;
    [Header("Dont change this:")]
    public GameObject currentSphere;


    //------------------------button functions------------------------
    //updates the mass when the value of the Inputfeild is cahnged
    public void ChangeMass()
    {
        currentSphere.GetComponent<Rigidbody>().mass = float.Parse(gameObject.GetComponent<RightClickHelper>().Mass.GetComponent<InputField>().text);
    }

    //updates the charge when the value of the Inputfeild is cahnged
    public void ChangeCharge()
    {
        currentSphere.GetComponent<charger>().charge = float.Parse(gameObject.GetComponent<RightClickHelper>().Charge.GetComponent<InputField>().text);
    }

    //Anchor toggle
    public void ToggleAnchor()
    {
        //if the sphere is frozen
        if (currentSphere.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll)
        {
            //un freeze it
            currentSphere.GetComponent<Rigidbody>().constraints = UnAnchorConstraints;
           //Debug.Log("froze");
        }
        //if the sphere is not frozen
        else
        {
            //freeze it
            currentSphere.GetComponent<Rigidbody>().constraints = AnchorConstraints;
            //and then stop it so it is for sure in place. sometimes an object will keep going even if it is frozen if it alredy has a force applided
            currentSphere.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
           //Debug.Log("unfreze");
        }
    }

    //color buttons
    //note- I tryed just using one function to control all colors, but the function dissapared when setting up the button
    public void ChangeColorRed()
    {
        currentSphere.GetComponent<Renderer>().material.color = Color.red;
    }
    public void ChangeColorBlue()
    {
        currentSphere.GetComponent<Renderer>().material.color = Color.blue;
    }
    public void ChangeColorGreen()
    {
        currentSphere.GetComponent<Renderer>().material.color = Color.green;
    }
    public void ChangeColorYellow()
    {
        currentSphere.GetComponent<Renderer>().material.color = Color.yellow;
    }

    //size buttona
    public void SetSize(float scale)
    {
        currentSphere.transform.localScale = new Vector3(scale, scale, scale);
    }

    //to add to all buttons to fix bug
    public void HideRightMenu()
    {
        rightMenu.SetActive(false);
    }

     public void CheckRightVisablity()
    {
        //Rect rightCanvasRect = new Rect(0, 0, gameObject.GetComponent<RectTransform>().rect.width * gameObject.GetComponent<Canvas>().scaleFactor, gameObject.GetComponent<RectTransform>().rect.height * gameObject.GetComponent<Canvas>().scaleFactor);
        float trigger2x = triggerPoint2.GetComponent<RectTransform>().position.x;
        float trigger2y = triggerPoint2.GetComponent<RectTransform>().position.y;
        //Rect rect = new Rect();
        rightMenu.transform.position = Input.mousePosition + new Vector3(rightMenu.GetComponent<RectTransform>().rect.width / 2f, -rightMenu.GetComponent<RectTransform>().rect.height / 2f, Camera.main.WorldToScreenPoint(gameObject.transform.position).z);
        //if the right menu it too far right
        if (trigger2y < gameObject.GetComponent<RectTransform>().rect.height * gameObject.GetComponent<Canvas>().scaleFactor && trigger2x > gameObject.GetComponent<RectTransform>().rect.width * gameObject.GetComponent<Canvas>().scaleFactor)
        {
            //Debug.Log("if the right menu it too far right");
            rightMenu.transform.position = Input.mousePosition + new Vector3(-rightMenu.GetComponent<RectTransform>().rect.width / 2f, -rightMenu.GetComponent<RectTransform>().rect.height / 2f, Camera.main.WorldToScreenPoint(gameObject.transform.position).z);
        }
        //if it is too far down
        if (trigger2y < gameObject.GetComponent<RectTransform>().rect.yMax * gameObject.GetComponent<Canvas>().scaleFactor && trigger2x < gameObject.GetComponent<RectTransform>().rect.width * gameObject.GetComponent<Canvas>().scaleFactor)
        {
            //Debug.Log("too far down");
            rightMenu.transform.position = Input.mousePosition + new Vector3(rightMenu.GetComponent<RectTransform>().rect.width / 2f, rightMenu.GetComponent<RectTransform>().rect.height / 2f, Camera.main.WorldToScreenPoint(gameObject.transform.position).z);
        }
        if (trigger2y < gameObject.GetComponent<RectTransform>().rect.yMax * gameObject.GetComponent<Canvas>().scaleFactor && trigger2x > gameObject.GetComponent<RectTransform>().rect.width * gameObject.GetComponent<Canvas>().scaleFactor)
        {
            //Debug.Log("both");
            rightMenu.transform.position = Input.mousePosition + new Vector3(-rightMenu.GetComponent<RectTransform>().rect.width / 2f, rightMenu.GetComponent<RectTransform>().rect.height / 2f, Camera.main.WorldToScreenPoint(gameObject.transform.position).z);
        }

        /*//if it is too far down and to the right
        if (trigger2x > gameObject.GetComponent<RectTransform>().rect.width * gameObject.GetComponent<Canvas>().scaleFactor && trigger2y > gameObject.GetComponent<RectTransform>().rect.height * gameObject.GetComponent<Canvas>().scaleFactor) 
        {
            // Inside
            //Debug.Log("both");
            rightMenu.transform.position = Input.mousePosition + new Vector3(-rightMenu.GetComponent<RectTransform>().rect.width / 2f, -rightMenu.GetComponent<RectTransform>().rect.height / 2f, Camera.main.WorldToScreenPoint(gameObject.transform.position).z);
        }*/
        //if it is perfect
    }
}



    /*//tempature buttons
    public void IncreaseTemp()
    {
        currentSphere.GetComponent<Rigidbody>().AddForce(new Vector3(currentSphere.GetComponent<Rigidbody>().velocity.x * 2, currentSphere.GetComponent<Rigidbody>().velocity.y *2, 0), ForceMode.Impulse);
    }
    public void DecreaseTemp()
    {
        currentSphere.GetComponent<Rigidbody>().velocity = new Vector3 (0,0,0);
    }*/
