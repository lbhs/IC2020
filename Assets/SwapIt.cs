using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapIt : MonoBehaviour
{
	[SerializeField]
	private Object PrefabToBecome; 
	
	void OnMouseOver()
	{
		Debug.Log(GetComponent<FixedJoint2D>());
		if(Input.GetMouseButtonDown(2) && GetComponent<BondMaker>().bonded == false)
		{
			Instantiate(PrefabToBecome, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
    }
}