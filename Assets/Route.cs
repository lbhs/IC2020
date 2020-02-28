using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IC2020;
using System.Linq;

public class Route : MonoBehaviour
{
	[SerializeField]
	public Transform[] controlPoints;

	private Vector2 gizmosPosition;
	
	private void OnDrawGizmos()
	{
		for (float t = 0; t <= 1; t += 0.025f)
		{
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
	
	//slow and inaccurate but it works
	public float nearestPointT(Vector2 pos, float accuracy)
	{
		float nearestT = 0f;
		float nearestDistance = Vector2.Distance(bezierPosition(0f), pos);
		float currentDistance;
		
		for(float t = 0f; t <= 1f; t += accuracy) //increment increases accuracy - need to make less resource intensive - use derivative?
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
	
	public float newtonsMethod(Vector2 point, float currentPos, int iterations)
	{
		float nextPos = currentPos - (funcDY(currentPos, point) / funcDDY(currentPos, point));
		print(nextPos);
		if(iterations == 1)
		{
			return nextPos;
		}
		return newtonsMethod(point, nextPos, iterations - 1);
	}
	
	public Vector2 nearestCurvePoint(Vector2 point)
	{
		List<Vector2> initial = new List<Vector2>(){bezierPosition(0f), bezierPosition(0.5f), bezierPosition(1f)};
		Vector2 quadraticEstimate = minFuncP(point, quadraticMethod(point, initial, 1));
		//float newtonsEstimate = newtonsMethod(point, nearestPointT(quadraticEstimate, 0.00001f), 4);
		return quadraticEstimate;
	}
	
	public float funcD(Vector2 point, Vector2 pos)
	{
		return ((pos.x - point.x) * (pos.x - point.x)) + ((pos.y - point.y) * (pos.y - point.y));
	}
	
	public float funcDY(float t, Vector2 pos)
	{
		Vector2 slope = bezierSlope(t);
		Vector2 point = bezierPosition(t);
		return 2 * ((pos.x - point.x) * slope.x) + ((pos.y - point.y) * slope.y);
	}
	
	public float funcDDY(float t, Vector2 pos)
	{
		Vector2 point = bezierPosition(t);
		Vector2 slope = bezierSlope(t);
		Vector2 acceleration = bezierAcceleration(t);
		return 2 * ((slope.x * slope.x)+(acceleration.x * (pos.x - point.x))+(slope.y * slope.y)+(acceleration.y * (pos.y - point.y)));
	}
	
	public float findTforPos(Vector2 point)
	{
		return 0;
	}
	
	public Vector2 minFuncP(Vector2 point, List<Vector2> slist)
	{
		Vector2 y23 = ((slist[1] * slist[1]) - (slist[2] * slist[2])) * funcD(point, slist[0]);
		Vector2 y31 = ((slist[2] * slist[2]) - (slist[0] * slist[0])) * funcD(point, slist[1]);
		Vector2 y12 = ((slist[0] * slist[0]) - (slist[1] * slist[1])) * funcD(point, slist[2]);
		Vector2 s23 = (slist[1] - slist[2]) * funcD(point, slist[0]);
		Vector2 s31 = (slist[2] - slist[0]) * funcD(point, slist[1]);
		Vector2 s12 = (slist[0] - slist[1]) * funcD(point, slist[2]);
		
		return 0.5f * ((y23 + y31 + y12)/(s23 + s31 + s12));
	}
	
	public List<Vector2> quadraticMethod(Vector2 point, List<Vector2> slist, int iterations)
	{
		Vector2 star = minFuncP(point, slist);
		slist.Add(star);
		//maybe rewrite to not create new list each time
		List<float> PValues = new List<float>(){funcP(slist[0], slist), funcP(slist[1], slist), funcP(slist[2], slist), funcP(star, slist)};
		foreach(float item in PValues)
		{
			print(item);
		}
		print(PValues.IndexOf(PValues.Max()));
		slist.RemoveAt(PValues.IndexOf(PValues.Max()));
		if(iterations == 1)
		{
			return slist;
		}
		return quadraticMethod(point, slist, iterations - 1);
	}
	
	public float funcP(Vector2 point, List<Vector2> slist)
	{
		Vector2 first = ((point - slist[1])*(point - slist[2])*funcD(point, slist[0]))/((slist[0] - slist[1])*(slist[0] - slist[2]));
		Vector2 second = ((point - slist[0])*(point - slist[2])*funcD(point, slist[1]))/((slist[1] - slist[0])*(slist[1] - slist[2]));
		Vector2 third = ((point - slist[0])*(point - slist[1])*funcD(point, slist[2]))/((slist[2] - slist[0])*(slist[2] - slist[1]));
		return funcD(point, (first + second + third));
	}
	
	public float funcB(int n, int i, float u)
	{
		return (factorial(n)/(factorial(i)*factorial(n - i))) * Mathf.Pow(1 - u, n - i) * Mathf.Pow(u, i);
	}
	
	public Vector2 bezierPosition(float t)
	{
		Vector2 summation = Vector2.zero;
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= n; i++)
		{
			summation += funcB(n, i, t) * (Vector2)controlPoints[i].position;
		}
		return summation;
	}
	
	public Vector2 bezierSlope(float t)
	{
		Vector2 summation = Vector2.zero;
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= (n - 1); i++)
		{
			summation += funcB(n-1, i, t) * n * (Vector2)(controlPoints[i+1].position - controlPoints[i].position);
		}
		return summation;
	}	
	
	public Vector2 bezierAcceleration(float t)
	{
		Vector2 summation = Vector2.zero;
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= (n - 2); i++)
		{
			summation += funcB(n-2, i, t) * n * (n-1) * (Vector2)(controlPoints[i+2].position - (2 * controlPoints[i+1].position) - controlPoints[i].position);
		}
		return summation;
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
