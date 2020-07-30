using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SwapItMobile : MonoBehaviour
{
    public GameObject PrefabToBecome;
    private Vector3 InitalPos;
    private float DistanceThreshold = 0.2f; //must be within 0.2 units of initial position to be swappable so it doesn't swap while atom is moving
    private float timeThreshold = 1.0f; //hold down for this many seconds to switch atom type
    private bool heldDown = false;
    private Coroutine currentTimer;

    
    

    private void OnMouseDown()
    {
        heldDown = true;
        InitalPos = transform.position;
        currentTimer = StartCoroutine(seeIfStillHeldDown());  //long click is used to swap atom forms
    }

    private void OnMouseUp()
    {
        heldDown = false;
        StopCoroutine(currentTimer);
    }

    private IEnumerator seeIfStillHeldDown()
    {
        {
            yield return new WaitForSeconds(timeThreshold);
            float distance = Vector3.Distance(InitalPos, transform.position);
            if (heldDown && distance < DistanceThreshold && gameObject.GetComponent<BondMaker>().bonded == false)
            {
                swapAtom();
                if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls ==1)
                {
                    DieScript.SwitchAtomMessageGiven++;   //This is used for tutorial purposes
                    if (DieScript.SwitchAtomMessageGiven >= 2 && gameObject.name == "CarbonE(Clone)")
                    {
                        GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().BlueJoulesArePE();
                    }

                }
                    
                
            }
            heldDown = false;
        }
    }

    public void swapAtom()
    {
		Instantiate(PrefabToBecome, transform.position, Quaternion.identity);
		Destroy(gameObject);
    }
}
