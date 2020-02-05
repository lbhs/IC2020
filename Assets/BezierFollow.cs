using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
	[SerializeField]
	private GameObject route;
	
	private Vector3 lastFrameVelocity;

    void Start()
    {
        transform.position = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position));
		//print(route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position))); 
    }
	
	void FixedUpdate()
	{
		//GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0));
		//print("adding force");
		//GetComponent<Rigidbody>().velocity += new Vector3(0,-9.81f,0) * Time.deltaTime;
		GetComponent<Rigidbody>().velocity = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position + GetComponent<Rigidbody>().velocity)) - new Vector2(transform.position.x, transform.position.y);
		transform.position = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position));
	}
	
	/* normal force bad
	void FixedUpdate()
	{
		//add if statement for when near bezier?
		
		float t = route.GetComponent<Route>().nearestPointT(transform.position);
		Vector2 tangent = route.GetComponent<Route>().bezierSlope(t);
		Vector2 normal = Quaternion.AngleAxis(90, Vector3.forward) * tangent;
		//print(normal.x + " " + normal.y);
		float magnitude = GetComponent<Rigidbody>().mass * Physics.gravity.y * (normal.x / normal.magnitude); //N = mgcosΘ
		//print(Mathf.Acos((normal.x / normal.magnitude)) * 57.2958f);
		Vector3 force = normal.normalized * magnitude;
		GetComponent<Rigidbody>().AddForce(force);
	}
	
	//normal force and elastic collision also bad
	void FixedUpdate()
	{
			float t = route.GetComponent<Route>().nearestPointT(transform.position);
			Vector2 tangent = route.GetComponent<Route>().bezierSlope(t);
			Vector2 normal = Quaternion.AngleAxis(90, Vector3.forward) * tangent;
			var speed = lastFrameVelocity.magnitude;
			var direction = Vector3.Reflect(lastFrameVelocity.normalized, normal);
			//transform.position += new Vector3(0, 5f, 0);
			GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			
			float magnitude = GetComponent<Rigidbody>().mass * Physics.gravity.y * (normal.x / normal.magnitude); //N = mgcosΘ
			Vector3 force = normal.normalized * magnitude;
			GetComponent<Rigidbody>().AddForce(force);
		
		lastFrameVelocity = GetComponent<Rigidbody>().velocity;
	}
	*/
}
