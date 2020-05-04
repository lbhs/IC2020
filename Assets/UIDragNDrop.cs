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

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        UseingMe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		bool ableToSpawn = true;
		
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
		
		foreach(var go in Physics2D.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Vector3.forward))
		{
			if(go.transform.gameObject.name.Contains("(Clone)")){ableToSpawn = false;}
		}
		
		if(ableToSpawn/*should be true when cursor isn't over UI or another game object*/)
		{ 
			if(AtomInventoryRemaining.removePiece(PrefabToSpawn, true) >= 1)
            {
				if (returnToZero == true)
				{
					transform.localPosition = Vector3.zero;
				}
				UseingMe = false;
				prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				prefabWorldPosition.z = 0;
				DieScript.rolling++;
				Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
				
				if(PrefabToSpawn.tag == "Diatomic")
				{
					DisplayJoules.BonusPointTotal += 10;  //if prefabToSpawn is diatomic, add 10 BonusPts (no need to add to MOLECULEID LIST?)
				}
				
				GameObject.Find("UI").GetComponent<Animator>().SetTrigger("Exit");
			}
			else
			{
				GameObject.Find("NotEnoughJoulesDisplay").GetComponent<CannotBreakBond>().OutOfInventory2();
				transform.localPosition = Vector3.zero;
			}
		}
		else
		{
			transform.localPosition = Vector3.zero;
		}
    }
}