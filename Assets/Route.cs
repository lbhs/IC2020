using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IC2020;

public class Route : MonoBehaviour
{
	[SerializeField]
	public Transform[] controlPoints;

	private Vector2 gizmosPosition;
	
	private void OnDrawGizmos()
	{
		for (float t = 0; t <= 1; t += 0.025f)
		{
			gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + Mathf.Pow(t, 3) * controlPoints[3].position;
			Gizmos.DrawSphere(bezierPosition(t), 0.25f);
		}
		
		//Gizmos.DrawSphere(bezierPosition(0.5f), 0.5f);
		/*
		for(int i = 0; i < controlPoints.Length; i += 2)
		{
			Gizmos.DrawLine(new Vector2(controlPoints[i].position.x, controlPoints[i].position.y), new Vector2(controlPoints[i+1].position.x, controlPoints[i+1].position.y));
		}
		*/
		
	}
	/* spawn a bunch of spheres along the curve
	void Start()
	{
		for (float t = 0; t <= 1; t += 0.005f)
		{
			GameObject curve = new Particle(pos: bezierPosition(t), mass: 0f, bounciness:0f).Spawn();
			curve.GetComponent<Rigidbody>().isKinematic = true;
			curve.GetComponent<MeshRenderer>().enabled = true;
		}
	}
	*/
	
	public float nearestPointT(Vector2 pos)
	{
		float nearestT = 0f;
		float nearestDistance = Vector2.Distance(bezierPosition(0f), pos);
		float currentDistance;
		
		for(float t = 0f; t <= 1f; t += 0.0001f) //increment increases accuracy - need to make less resource intensive - use derivative?
		{
			currentDistance = Vector2.Distance(bezierPosition(t), pos);
			if(currentDistance < nearestDistance)
			{
				nearestDistance = currentDistance;
				nearestT = t;
			}
		}
		return nearestT;
	}
	
	public float distanceFromCurve(Vector2 pos)
	{
		return Vector2.Distance(pos, bezierPosition(nearestPointT(pos)));
	}
	
	public Vector2 bezierPosition(float t)
	{
		Vector2 summation = new Vector2(0,0);
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= n; i++)
		{
			float binomCoefficient = (factorial(n)/(factorial(i)*factorial(n - i)));
			float mainPower = Mathf.Pow(1 - t, n - i);
			float secondPower = Mathf.Pow(t, i);
			Vector2 point = controlPoints[i].position;
			summation += binomCoefficient * mainPower * secondPower * point;
		}
		return summation;
	}
	
	public Vector2 bezierSlope(float t)
	{
		Vector2 summation = new Vector2(0,0);
		int n = controlPoints.Length - 1;
		for(int i = 0; i < n; i++)
		{
			float binomCoefficient = (factorial(n-1)/(factorial(i)*factorial(n - (i+1))));
			float mainPower = Mathf.Pow(1 - t, n - (i+1));
			float secondPower = Mathf.Pow(t, i);
			Vector2 point1 = controlPoints[i].position;
			Vector2 point2 = controlPoints[i+1].position;
			
			summation += binomCoefficient * mainPower * secondPower * n * (point2 - point1);
		}
		return summation;
		//return (summation.y/summation.x); //float version
	}
	
	public static int factorial(int number)
	{
		int val = 1;
		for(int i = 1; i <= number; i++)
		{
			val *= i;
		}
		return val;
	}
}
