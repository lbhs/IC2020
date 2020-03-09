﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
	[SerializeField]
	private GameObject route;
	
	public float speed;
	public float t = 0f;
	public bool strictMovement = true;
	private List<double> realPosition;
	private List<double> realVelocity;

    void Start()
    {
		realPosition = route.GetComponent<Route>().toList(transform.position);
		print(route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position, 0.00000001f))[0]);
		print(route.GetComponent<Route>().nearestCurvePoint(realPosition)[0]);
	}
	
	void FixedUpdate()
	{
		if(strictMovement)
		{
			StartCoroutine(GoByTheRoute());
		} else {
			updatePosition();
		}
		
		if(GetComponent<TimeBody>().frame == -1)
		{
			GetComponent<Rigidbody>().isKinematic = false;
		}
	}
	
	void updatePosition()
	{
		
		//artificial gravity - should exactly mimic rigidbody gravity but here just in case if we want to make sure gravity is calculated BEFORE the position adjustments
		//GetComponent<Rigidbody>().velocity += new Vector3(0,-9.81f,0) * Time.deltaTime;
		
		List<double> nextPos = route.GetComponent<Route>().nearestCurvePoint(route.GetComponent<Route>().combine(realPosition, realVelocity));
		realVelocity = route.GetComponent<Route>().decombine(nextPos, realPosition);
		realPosition = nextPos;
		
		transform.position = route.GetComponent<Route>().toVector(realPosition);
		GetComponent<Rigidbody>().velocity = route.GetComponent<Route>().toVector(realVelocity);
	}
	
	private IEnumerator GoByTheRoute()
	{
		strictMovement = false;
		while(t < 1f)
		{
			while(GetComponent<TimeBody>().isRewinding)
			{
				yield return null;
			}
			
			t += Time.deltaTime * speed;
			transform.position = route.GetComponent<Route>().toVector(route.GetComponent<Route>().bezierPosition(t));
			
			yield return new WaitForEndOfFrame();
		}
		
		transform.position = new Vector3(-15, 0, -15);
		GetComponent<Rigidbody>().isKinematic = true;
	}
}