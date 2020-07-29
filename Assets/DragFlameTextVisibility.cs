using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragFlameTextVisibility : MonoBehaviour
{
    public Text DragFlameTextBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GOAL IS TO MAKE THE TEXT "Drag this Flame Icon to Break Bonds" visible only when there are bonds to be broken (e.g. on turn 2 or 3 or 4)

        if (DieScript.totalRolls > 1 && DieScript.totalRolls < 5 && DisplayCanvasScript.JouleTotal >0 && GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == false)
        {
            DisplayDragText();
        }
        else
        {
            DisplayStandardText();
        }
    }

    void DisplayDragText()
    {
        DragFlameTextBox.text = "Drag this Flame Icon to Break Bonds";         
    }

    void DisplayStandardText()
    {
        DragFlameTextBox.text = "Bond-breaking Energy";
    }

}
