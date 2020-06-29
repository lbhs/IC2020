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
		if(gameObject.GetComponent<BondMaker>().bonded == false && holdTime < holdTimeThreshold)
		{
			transform.Rotate(0, 0, 90);
		}
    }
}
