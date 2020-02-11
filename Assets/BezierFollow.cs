using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
	[SerializeField]
	private GameObject route;
	
	public float speed;
	private Vector3 lastFrameVelocity;
	public float t = 0f;
	public bool strictMovement = true;

    void Start()
    {
        transform.position = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position));
		
		//print(route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position))); 
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
		//GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0));
		//print("adding force");
		//GetComponent<Rigidbody>().velocity += new Vector3(0,-9.81f,0) * Time.deltaTime;
		GetComponent<Rigidbody>().velocity = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position + GetComponent<Rigidbody>().velocity)) - new Vector2(transform.position.x, transform.position.y);
		Vector3 nextPos = route.GetComponent<Route>().bezierPosition(route.GetComponent<Route>().nearestPointT(transform.position));
		//Debug.DrawLine(transform.position, nextPos, Color.black, false);
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
			
			t += Time.deltaTime * speed;
			transform.position = route.GetComponent<Route>().bezierPosition(t);
			
			yield return new WaitForEndOfFrame();
		}
		
		transform.position = new Vector3(-15, 0, -15);
		GetComponent<Rigidbody>().isKinematic = true;
	}
}
