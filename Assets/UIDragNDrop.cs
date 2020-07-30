using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool returnToZero = false; //default value is false
    public GameObject PrefabToSpawn;
    public bool UseingMe;
	Vector3 prefabWorldPosition;
    public static int BondableAtomsTakenSoFar;
    


    void Start()
	{
		GameObject.Find("UI").GetComponent<Animator>().SetBool("Exiting", false);
	}

    public void OnDrag(PointerEventData eventData)
    {
		if(!GameObject.Find("UI").GetComponent<Animator>().GetBool("Exiting"))
		{
			transform.position = Input.mousePosition;
			UseingMe = true;
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		prefabWorldPosition.z = 0;
		
		if(ableToSpawn() && !GameObject.Find("UI").GetComponent<Animator>().GetBool("Exiting"))
		{
            bool CanRemovePiece = false;
            if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 3)
            {
                CanRemovePiece = AtomInventoryRemaining.removePiece(GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Turn3Prefab, true);
            }
            else
            {
                CanRemovePiece = AtomInventoryRemaining.removePiece(PrefabToSpawn, true);
            }

                /*if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 3)
                {
                    //tutorial requests that the user choose diatomic H2 on turn 3  THIS BAD SCRIPT MADE CL2 PERMANENTLY SPAWN H2!!!!
                    PrefabToSpawn = GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Turn3Prefab;
                    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().BreakABond();
                }*/

            if (CanRemovePiece)  //this means that there is inventory of this piece available
            {   
				if (returnToZero == true)
				{
					transform.localPosition = Vector3.zero;
				}
				UseingMe = false;
                if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 0)
                {
                    //don't think we need this now that "rolling" is set to 0 in the ConversationTextDisplayScript
                }
                else
                { DieScript.rolling++; }

                if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 3)
                {
                    //tutorial requests that the user choose diatomic H2
                    Instantiate(GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Turn3Prefab, prefabWorldPosition, Quaternion.identity);
                    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().BreakABond();
                }
                else
                {
                    Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
                }
                

                if (DieScript.totalRolls == 12)  //this triggers the message that the game is about to end!
                {
                    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().finalTurn();
                }                         



                if (PrefabToSpawn.tag == "Diatomic")
                {
                    DisplayCanvasScript.BonusPointTotal += 10;  //if prefabToSpawn is diatomic, add 10 BonusPts (no need to add to MOLECULEID LIST?)
                    
                }

                else if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true)
                {//ALL THE SCRIPTS BELOW RUN ONLY DURING TUTORIAL ROUND!
                    //BondableAtomsTakenSoFar++;
                    //print("Bondable Atoms = " + BondableAtomsTakenSoFar);


                    if (DieScript.totalRolls == 1 && DieScript.RotateMessageGiven == 0)  
                    {
                        GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().RotateAtomInstructions();
                        //value of RotateMessageGiven is changed in the RotateThis script!  Indicates the player has done at least one rotation
                        //that triggers the "SwitchAtomForm" message to be transmitted next
                    }

                    if (DieScript.totalRolls == 1 && DieScript.RotateMessageGiven >= 2)  //UIDragNDrop.BondableAtomsTakenSoFar > 0 && 
                    {
                        GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().SwitchAtomForm();
                        //DieScript.SwitchAtomMessageGiven is incremented in SwapIt script  When value reaches 3, Roll Die message initiated;
                    }


                    if (DieScript.totalRolls == 2) //UIDragNDrop.BondableAtomsTakenSoFar == 2 && 
                    {
                        if(PrefabToSpawn.name == "OxygenE")
                        {
                            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().SwitchFormToOxygenEB();
                        }
                        else
                        {
                            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().BondingConvertsPEtoHeat();
                        }
                        //Tutorial round gives the player a carbon in round 0 and an oxygen in round 1--instruction is to make double bond
                    }
                }

                GameObject.Find("UI").GetComponent<Animator>().SetTrigger("Exit");
				GameObject.Find("UI").GetComponent<Animator>().SetBool("Exiting", true);
			}
			else
			{
				GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().OutOfInventory2();
				transform.localPosition = Vector3.zero;
			}
		}
		else
		{
			transform.localPosition = Vector3.zero;
		}
    }
	
	public bool ableToSpawn()
	{
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer, raycastResults);
		if(raycastResults.Count > 0)
		{
			foreach(var go in raycastResults)
			{
				if(go.gameObject.transform.parent.gameObject.name == "RollPannelSingle" || go.gameObject.transform.parent.gameObject.name == "RollPannelDouble")
				{
					return false;
				}
			}
		}
		
		GameObject dummyObject = Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
		int accuracy = 5; //1 is pixel perfect accuracy but causes stutter, 5 is a great performance but could allow minor overlap
		int range = Screen.height/2;
		
		for(int x = (int)Input.mousePosition.x - range; x < (int)Input.mousePosition.x + range; x+=accuracy)
		{
			for(int y = (int)Input.mousePosition.y - range; y < (int)Input.mousePosition.y + range; y+=accuracy)
			{
				RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, Vector2.zero);
				//Debug.DrawRay(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, transform.TransformDirection(Vector3.forward) * 100, Color.green, 10f, false);
				if(hits.Length > 1)
				{
					foreach(var go in hits)
					{
						if(go.rigidbody.gameObject == dummyObject)
						{
							foreach(var go2 in hits)
							{
								if(go2.rigidbody.gameObject != dummyObject && (go2.rigidbody.gameObject.GetComponent<DragIt>() != null || go2.rigidbody.gameObject.tag == "Diatomic"))
								{
									Destroy(dummyObject);
									Debug.DrawRay(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, transform.TransformDirection(Vector3.forward) * 100, Color.green, 10f, false);
									GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().noStack();
									return false;
								}
							}
						}
					}
				}
			}
		}
		Destroy(dummyObject);
		return true;
	}
}