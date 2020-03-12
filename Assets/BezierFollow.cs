using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
	[SerializeField]
	private GameObject route;
	
	public float speed;
	public float t = 0f;
	public bool strictMovement = true;
	private double x;
	private double y;

    void Start()
    {
		transform.position = route.GetComponent<Route>().nearestCurvePoint(transform.position);
		//print(route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position, 0.000001f)).x);
		//print(route.GetComponent<Route>().nearestCurvePoint(transform.position).x);
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
		Vector3 nextPos = route.GetComponent<Route>().nearestCurvePoint(transform.position + (GetComponent<Rigidbody>().velocity*Time.fixedDeltaTime));
		GetComponent<Rigidbody>().velocity = (nextPos - transform.position)/Time.fixedDeltaTime;
		transform.position = nextPos;
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
			
			t += Time.fixedDeltaTime * speed;
			transform.position = route.GetComponent<Route>().bezierPosition(t);
			
			yield return new WaitForEndOfFrame();
		}
		
		transform.position = new Vector3(-15, 0, -15);
		GetComponent<Rigidbody>().isKinematic = true;
	}
}
