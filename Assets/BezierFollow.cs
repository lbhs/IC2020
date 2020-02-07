using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
	[SerializeField]
	private GameObject route;
	
	public float speed;
	private Vector3 lastFrameVelocity;
	private float t = 0f;
	public bool strictMovement = true;

    void Start()
    {
        transform.position = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position));
		
		//print(route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position))); 
    }
	/*
	void FixedUpdate()
	{
		//GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0));
		//print("adding force");
		//GetComponent<Rigidbody>().velocity += new Vector3(0,-9.81f,0) * Time.deltaTime;
		GetComponent<Rigidbody>().velocity = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position + GetComponent<Rigidbody>().velocity)) - new Vector2(transform.position.x, transform.position.y);
		Vector3 nextPos = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position));
		Debug.DrawLine(transform.position, nextPos, depthTest: false);
		transform.position = nextPos;
	}
	*/
	
	void Update()
    {
        if(strictMovement)
		{
			StartCoroutine(GoByTheRoute());
		}
    }
	
	private IEnumerator GoByTheRoute()
	{
		strictMovement = false;
		while(t < 1f)
		{
			t += Time.deltaTime * speed;
			transform.position = route.GetComponent<Route>().bezierPosition(t);
			
			yield return new WaitForEndOfFrame();
		}
		
		transform.position = new Vector3(-15, 0, -15);
		GetComponent<Rigidbody>().isKinematic = true;
		
		strictMovement = true;
	}
	
	void FixedUpdate()
	{		
		if(GetComponent<TimeBody>().frame == -1)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			//mainObject.gameObjects.Add(gameObject);
		}
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


