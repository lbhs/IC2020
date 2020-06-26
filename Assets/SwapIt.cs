using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SwapIt : MonoBehaviour
{	
	public GameObject PrefabToBecome;	
    private Vector3 InitalPos;
    private float DistanceThreshold = 0.2f; //must be within 0.2 units of initial position to be swappable so it doesn't swap while atom is moving
	private float timeThreshold = 0.8f; //hold down for 0.8 seconds to switch atom type
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
		yield return new WaitForSeconds(timeThreshold);
		float distance = Vector3.Distance(InitalPos, transform.position);
		if(heldDown && distance < DistanceThreshold && gameObject.GetComponent<BondMaker>().bonded == false)
		{
			swapAtom();
		}
		heldDown = false;
	}
	
	void Update()
	{
		if(Input.GetMouseButtonDown(1))
		{
			swapAtom();
		}
	}
	
	public void swapAtom()
	{
		switch(AtomInventoryRemaining.removePiece(PrefabToBecome, true))
		{
			case 0:
				if(PrefabToBecome.GetComponent<SwapIt>().PrefabToBecome.name + "(Clone)" == gameObject.name || AtomInventoryRemaining.pieceToName(gameObject) == "OxygenEB(Clone)")
				{
					GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().OutOfInventory3();
				} else {
					Instantiate(PrefabToBecome.GetComponent<SwapIt>().PrefabToBecome, transform.position, Quaternion.identity);
					Destroy(gameObject);
				}
				break;
			case 1:
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