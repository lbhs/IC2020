using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
	[SerializeField]
	private GameObject route;
	
	Route rt;
	Rigidbody rb;
	
	void OnStart()
	{
		rt = route.GetComponent<Route>();
		rb = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        transform.position = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position));
    }
	/*
	void FixedUpdate()
	{
		//add if statement for when near bezier?
		float t = rt.nearestPointT(transform.position);
		Vector2 tangent = rt.bezierSlope(t);
		Vector2 normal = Quaternion.AngleAxis(90, Vector3.forward) * tangent;
		float magnitude = 
		Vector3 force = normal.normalized * magnitude;
		rb.AddForce(force);
	}
	*/
}
