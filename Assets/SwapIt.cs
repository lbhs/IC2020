using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SwapIt : MonoBehaviour
{	
	public GameObject PrefabToBecome;
	private float LastClickTime;
	private float TimeSinceLastClick;
	public const double DoubleClickThreshold = 0.2;
	private static GameObject prefab;
	
	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0) && GetComponent<BondMaker>().bonded == false)
		{
			TimeSinceLastClick = Time.time - LastClickTime;
            if (TimeSinceLastClick <= DoubleClickThreshold)
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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LastClickTime = Time.time;
        }
    }
}