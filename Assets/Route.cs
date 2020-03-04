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
		for (double t = 0; t <= 1; t += 0.025f)
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
	public double nearestPointT(Vector2 pos, double accuracy)
	{
		double nearestT = 0f;
		double nearestDistance = Vector2.Distance(bezierPosition(0f), pos);
		double currentDistance;
		
		for(double t = 0f; t <= 1f; t += accuracy) //increment increases accuracy - need to make less resource intensive - use derivative?
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
	
	public double newtonsMethod(Vector2 point, double currentPos, int iterations)
	{
		double nextPos = currentPos - (funcDY(currentPos, point) / funcDDY(currentPos, point));
		print(bezierPosition(nextPos).x);
		if(iterations == 1)
		{
			return nextPos;
		}
		return newtonsMethod(point, nextPos, iterations - 1);
	}
	
	public Vector2 nearestCurvePoint(Vector2 point)
	{
		List<double> initial = new List<double>(){0, 0.5, 1};
		double quadraticEstimate = minFuncP(point, quadraticMethod(point, initial, 5));
		//print(bezierPosition(quadraticEstimate).x);
		double newtonsEstimate = newtonsMethod(point, quadraticEstimate, 5);
		return bezierPosition(quadraticEstimate);
	}
	
	public double funcD(double t, Vector2 pos)
	{
		Vector2 point = bezierPosition(t);
		return ((pos.x - point.x) * (pos.x - point.x)) + ((pos.y - point.y) * (pos.y - point.y));
	}
	
	public double funcDY(double t, Vector2 pos)
	{
		Vector2 point = bezierPosition(t);
		Vector2 slope = bezierSlope(t);
		return 2 * ((point.x - pos.x) * slope.x) + ((point.y - pos.y) * slope.y);
	}
	
	public double funcDDY(double t, Vector2 pos)
	{
		Vector2 point = bezierPosition(t);
		Vector2 slope = bezierSlope(t);
		Vector2 acceleration = bezierAcceleration(t);
		return 2 * ((slope.x * slope.x)+(acceleration.x * (point.x - pos.x))+(slope.y * slope.y)+(acceleration.y * (point.y - pos.y)));
	}
	
	public double minFuncP(Vector2 point, List<double> slist)
	{
		double y23 = ((slist[1] * slist[1]) - (slist[2] * slist[2])) * funcD(slist[0], point);
		double y31 = ((slist[2] * slist[2]) - (slist[0] * slist[0])) * funcD(slist[1], point);
		double y12 = ((slist[0] * slist[0]) - (slist[1] * slist[1])) * funcD(slist[2], point);
		double s23 = (slist[1] - slist[2]) * funcD(slist[0], point);
		double s31 = (slist[2] - slist[0]) * funcD(slist[1], point);
		double s12 = (slist[0] - slist[1]) * funcD(slist[2], point);
		
		return 0.5f * ((y23 + y31 + y12)/(s23 + s31 + s12));
	}
	
	public List<double> quadraticMethod(Vector2 point, List<double> slist, int iterations)
	{
		double star = minFuncP(point, slist);
		slist.Add(star);
		//maybe rewrite to not create new list each time
		List<double> PValues = new List<double>(){funcP(slist[0], slist, point), funcP(slist[1], slist, point), funcP(slist[2], slist, point), funcP(slist[3], slist, point)};
		/*debugging info
		foreach(double item in PValues)
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
	
	public double funcP(double t, List<double> slist, List<double> point)
	{
		double first = ((t - slist[1])*(t - slist[2])*funcD(slist[0], point))/((slist[0] - slist[1])*(slist[0] - slist[2]));
		double second = ((t - slist[0])*(t - slist[2])*funcD(slist[1], point))/((slist[1] - slist[0])*(slist[1] - slist[2]));
		double third = ((t - slist[0])*(t - slist[1])*funcD(slist[2], point))/((slist[2] - slist[0])*(slist[2] - slist[1]));
		return funcD((first + second + third), point);
	}
	
	public double funcB(int n, int i, double u)
	{
		return (factorial(n)/(factorial(i)*factorial(n - i))) * Math.Pow(1 - u, n - i) * Math.Pow(u, i);
	}
	
	public Vector2 bezierPosition(double t)
	{
		Vector2 summation = Vector2.zero;
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= n; i++)
		{
			summation += funcB(n, i, t) * (Vector2)controlPoints[i].position;
		}
		return summation;
	}
	
	public Vector2 bezierSlope(double t)
	{
		Vector2 summation = Vector2.zero;
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= (n - 1); i++)
		{
			summation += funcB(n-1, i, t) * n * (Vector2)(controlPoints[i+1].position - controlPoints[i].position);
		}
		return summation;
	}	
	
	public Vector2 bezierAcceleration(double t)
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
