using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool returnToZero = false; //default value is false
    public GameObject PrefabToSpawn;
    private Vector3 prefabWorldPosition;
    public bool UseingMe;

	void Start()
	{
		GameObject.Find("UI").GetComponent<Animator>().SetBool("Exit", false);
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
		bool ableToSpawn = true;
		prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer, raycastResults);
		if(raycastResults.Count > 0)
		{
			foreach(var go in raycastResults)
			{
				if(go.gameObject.transform.parent.gameObject.name == "RollPannelSingle"){ableToSpawn = false;}
				if(go.gameObject.transform.parent.gameObject.name == "RollPannelDouble"){ableToSpawn = false;}
			}
		}

        /*
        //new method at raycasting overlap prevention, checks entire atom for overlap
        if (ableToSpawn)
        {
            GameObject dummyObject = Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
            int range = (int)(Screen.width * 0.15);
            int accuracy = 5; //1 is pixel perfect accuracy but causes freezing, 5 is a great performance but could allow minor overlap

            for (int x = (int)Input.mousePosition.x - range; x < (int)Input.mousePosition.x + range; x += accuracy)
            {
                for (int y = (int)Input.mousePosition.y - range; y < (int)Input.mousePosition.y + range; y += accuracy)
                {
                    foreach (var go in Physics2D.RaycastAll(Camera.main.ScreenPointToRay(new Vector3(x, y, Input.mousePosition.y)).origin, Vector3.forward))
                    {
                        if (go == dummyObject)
                        {
                            if (Physics2D.RaycastAll(Camera.main.ScreenPointToRay(new Vector3(x, y, Input.mousePosition.y)).origin, Vector3.forward).Length > 1)
                            {
                                ableToSpawn = false;
                                GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().noStack();
                            }
                        }
                    }
                }
            }
            Destroy(dummyObject);
        }*/

		if(ableToSpawn/*should be true when cursor isn't over UI or another game object*/)
		{ 
			if(AtomInventoryRemaining.removePiece(PrefabToSpawn, true) >= 1)
            {
				if (returnToZero == true)
				{
					transform.localPosition = Vector3.zero;
				}
				UseingMe = false;
				prefabWorldPosition.z = 0;
				DieScript.rolling++;
				Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
				
				if(PrefabToSpawn.tag == "Diatomic")
				{
					DisplayJoules.BonusPointTotal += 10;  //if prefabToSpawn is diatomic, add 10 BonusPts (no need to add to MOLECULEID LIST?)
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
			transform.localPosition = Vector3.zero;  //UI object snaps back to where it came from
		}
    }
}