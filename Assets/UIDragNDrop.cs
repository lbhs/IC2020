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
			if(AtomInventoryRemaining.removePiece(PrefabToSpawn, true) >= 1)
            {
				if (returnToZero == true)
				{
					transform.localPosition = Vector3.zero;
				}
				UseingMe = false;
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