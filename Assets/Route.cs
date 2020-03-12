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
	public float Length;
	
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
	
	public void Start()
	{
		float tempLength = 0f;
		float accuracy = 0.000001f;
		for(float t = 0; t <= 1f - accuracy; t += accuracy)
		{
			tempLength += Vector3.Distance(bezierPosition(t), bezierPosition(t + accuracy));
		}
		Length = tempLength;
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
		//print(bezierPosition(nextPos).x);
		if(iterations == 1)
		{
			return nextPos;
		}
		return newtonsMethod(point, nextPos, iterations - 1);
	}
	
	public Vector3 nearestCurvePoint(Vector2 point)
	{
		List<float> initial = new List<float>(){0f, 0.5f, 1f};
		float quadraticEstimate = minFuncP(point, quadraticMethod(point, initial, 5));
		//print(bezierPosition(quadraticEstimate).x);
		float newtonsEstimate = newtonsMethod(point, quadraticEstimate, 5);
		return bezierPosition(newtonsEstimate);
	}
	
	public float funcD(float t, Vector2 pos)
	{
		Vector2 point = bezierPosition(t);
		return ((pos.x - point.x) * (pos.x - point.x)) + ((pos.y - point.y) * (pos.y - point.y));
	}
	
	public float funcDY(float t, Vector2 pos)
	{
		Vector2 point = bezierPosition(t);
		Vector2 slope = bezierSlope(t);
		return 2 * ((point.x - pos.x) * slope.x) + ((point.y - pos.y) * slope.y);
	}
	
	public float funcDDY(float t, Vector2 pos)
	{
		Vector2 point = bezierPosition(t);
		Vector2 slope = bezierSlope(t);
		Vector2 acceleration = bezierAcceleration(t);
		return 2 * ((slope.x * slope.x)+(acceleration.x * (point.x - pos.x))+(slope.y * slope.y)+(acceleration.y * (point.y - pos.y)));
	}
	
	public float minFuncP(Vector2 point, List<float> slist)
	{
		float y23 = ((slist[1] * slist[1]) - (slist[2] * slist[2])) * funcD(slist[0], point);
		float y31 = ((slist[2] * slist[2]) - (slist[0] * slist[0])) * funcD(slist[1], point);
		float y12 = ((slist[0] * slist[0]) - (slist[1] * slist[1])) * funcD(slist[2], point);
		float s23 = (slist[1] - slist[2]) * funcD(slist[0], point);
		float s31 = (slist[2] - slist[0]) * funcD(slist[1], point);
		float s12 = (slist[0] - slist[1]) * funcD(slist[2], point);
		
		return 0.5f * ((y23 + y31 + y12)/(s23 + s31 + s12));
	}
	
	public List<float> quadraticMethod(Vector2 point, List<float> slist, int iterations)
	{
		float star = minFuncP(point, slist);
		slist.Add(star);
		//maybe rewrite to not create new list each time
		List<float> PValues = new List<float>(){funcP(slist[0], slist, point), funcP(slist[1], slist, point), funcP(slist[2], slist, point), funcP(slist[3], slist, point)};
		/*debugging info
		foreach(float item in PValues)
		{
			print(item);
		}
		print(PValues.IndexOf(PValues.Max()));
		*/
		slist.RemoveAt(PValues.IndexOf(PValues.Max()));
		if(iterations == 1)
		{
			return slist;
		}
		return quadraticMethod(point, slist, iterations - 1);
	}
	
	public float funcP(float t, List<float> slist, Vector2 point)
	{
		float first = ((t - slist[1])*(t - slist[2])*funcD(slist[0], point))/((slist[0] - slist[1])*(slist[0] - slist[2]));
		float second = ((t - slist[0])*(t - slist[2])*funcD(slist[1], point))/((slist[1] - slist[0])*(slist[1] - slist[2]));
		float third = ((t - slist[0])*(t - slist[1])*funcD(slist[2], point))/((slist[2] - slist[0])*(slist[2] - slist[1]));
		return funcD((first + second + third), point);
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
