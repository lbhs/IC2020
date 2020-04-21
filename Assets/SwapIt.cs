using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapIt : MonoBehaviour
{
	[SerializeField]
	private Object PrefabToBecome; 
	
	private float LastClickTime;
	private float TimeSinceLastClick;
	public const double DoubleClickThreshold = 0.2;
	
	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0) && GetComponent<BondMaker>().bonded == false)
		{
			TimeSinceLastClick = Time.time - LastClickTime;
            if (TimeSinceLastClick <= DoubleClickThreshold)
            {
				Instantiate(PrefabToBecome, transform.position, Quaternion.identity);
				Destroy(gameObject);
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