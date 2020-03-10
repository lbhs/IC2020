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
	private List<double> realPosition;
	private List<double> realVelocity = new List<double>(){0,0,0};

    void Start()
    {
		realPosition = route.GetComponent<Route>().toList(transform.position);
		print(route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position, 0.000001f))[0]);
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
		
		//add fake velocity change to real velocity
		realVelocity = route.GetComponent<Route>().combine(route.GetComponent<Route>().toList(GetComponent<Rigidbody>().velocity), realVelocity);
		
		//calculate new position on curve based on velocity
		List<double> nextPos = route.GetComponent<Route>().nearestCurvePoint(route.GetComponent<Route>().combine(realPosition, realVelocity));
		
		//keep track of change in velocity that keeping it on the curve has induced
		realVelocity = route.GetComponent<Route>().decombine(nextPos, realPosition);
		
		//update accurate position for next frame
		realPosition = nextPos;
		
		//update actual visual of particle with the right position
		transform.position = route.GetComponent<Route>().toVector(realPosition);
		
		//keep visual velocity on 0 so it stays on the curve (velocity is still accounted for behind the scenes)
		//GetComponent<Rigidbody>().velocity = Vector3.zero;
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
