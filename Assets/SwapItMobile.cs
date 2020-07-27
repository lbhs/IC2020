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
        currentTimer = StartCoroutine(seeIfStillHeldDown());
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

    void Update()
    {
        
    }

    public void swapAtom()
    {
        switch (AtomInventoryRemaining.removePiece(PrefabToBecome, true))   //AtomInventoryRemaining script is attached to UI GameObject
        {

            case 0:  //switching from oxygen linear to bent, but not to double bond (OxygenEB)
                if (PrefabToBecome.GetComponent<SwapItMobile>().PrefabToBecome.name + "(Clone)" == gameObject.name || AtomInventoryRemaining.pieceToName(gameObject) == "OxygenEB(Clone)")
                {
                    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().OutOfInventory3();  //no atoms of this type remain
                }
                else
                {
                    Instantiate(PrefabToBecome.GetComponent<SwapItMobile>().PrefabToBecome, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
            case 1:   //removePiece function returns a 0, 1 or 2 depending on the specific case.  Case 1 means swap is possible.
                AtomInventoryRemaining.addPiece(AtomInventoryRemaining.pieceToName(gameObject));
                Instantiate(PrefabToBecome, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
            case 2:
                Instantiate(PrefabToBecome, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }
}
