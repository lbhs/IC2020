using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThis : MonoBehaviour
{
    private float holdTime = 0;
    public const double holdTimeThreshold = 0.2; //let go within 0.2 seconds to rotate

    private void OnMouseDown()
    {
        holdTime = 0;
    }
    void Update()
    {  
        holdTime += Time.deltaTime;
    }
    private void OnMouseUp()
    {
        if (JewelMover.JewelsInMotion == true)
        {  //disable rotation while jewels are moving
            print("Can't rotate while jewels are moving");
            return;
        }  

        else if (gameObject.GetComponent<BondMaker>().bonded == false && holdTime < holdTimeThreshold)
        {
            transform.Rotate(0, 0, 90);
            DieScript.RotateMessageGiven ++;
            if(DieScript.totalRolls==1 && DieScript.RotateMessageGiven >3)
            {
                GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().SwitchAtomForm();
            }
        }
    }
}

