using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThis : MonoBehaviour
{
	private float LastClickTime = 0;
	private float TimeSinceLastClick;
	public const double DoubleClickThreshold = 0.5; //must tap/click twice within 0.5 seconds to rotate
    private void OnMouseDown()
    {
		if(gameObject.GetComponent<BondMaker>().bonded == false)
		{
			TimeSinceLastClick = Time.time - LastClickTime;
			if(TimeSinceLastClick <= DoubleClickThreshold)
			{
				transform.Rotate(0, 0, 90);
			}
			else
			{
				LastClickTime = Time.time;
			}
		}
    }
}
