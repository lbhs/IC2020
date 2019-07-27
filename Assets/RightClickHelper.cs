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
    public GameObject anchorToggle;
    public GameObject toggleGroup;
    public GameObject Color1;
    public GameObject Color2;
    public GameObject Color3;
    public GameObject Color4;
    public GameObject Size1;
    public GameObject Size2;
    public RigidbodyConstraints AnchorConstraints;
    public RigidbodyConstraints UnAnchorConstraints;
    public GameObject triggerPoint; //possibly useless
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
        currentSphere.GetComponent<charger>().charge = int.Parse(gameObject.GetComponent<RightClickHelper>().Charge.GetComponent<InputField>().text);
    }

    //Anchor toggle
    public void ToggleAnchor()
    {/*
        if (anchorToggle.GetComponent<Toggle>().isOn == false)
        {
            currentSphere.GetComponent<Rigidbody>().constraints = AnchorConstraints;
            currentSphere.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Debug.Log("froze");
            anchorToggle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            currentSphere.GetComponent<Rigidbody>().constraints = UnAnchorConstraints;
            Debug.Log("unfreze");
            anchorToggle.GetComponent<Toggle>().isOn = false;
        }*/
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
