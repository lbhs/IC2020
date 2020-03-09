using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IC2020;
using System.Linq;
using System;

public class Route : MonoBehaviour
{
	[SerializeField]
	public Transform[] controlPoints;

	private Vector2 gizmosPosition;
	
	private void OnDrawGizmos()
	{
		for (double t = 0; t <= 1; t += 0.025f)
		{
			Gizmos.DrawSphere(toVector(bezierPosition(t)), 0.25f);
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
		float nearestDistance = Vector2.Distance(toVector(bezierPosition(0f)), pos);
		float currentDistance;
		
		for(float t = 0f; t <= 1f; t += accuracy) //increment increases accuracy - need to make less resource intensive - use derivative?
		{
			currentDistance = Vector2.Distance(toVector(bezierPosition(t)), pos);
			if(currentDistance < nearestDistance)
			{
				nearestDistance = currentDistance;
				nearestT = t;
			}
		}
		return nearestT;
	}
	
	public double newtonsMethod(List<double> point, double currentPos, int iterations)
	{
		double nextPos = currentPos - (funcDY(currentPos, point) / funcDDY(currentPos, point));
		print(bezierPosition(nextPos)[0]);
		if(iterations == 1)
		{
			return nextPos;
		}
		return newtonsMethod(point, nextPos, iterations - 1);
	}
	
	public List<double> nearestCurvePoint(List<double> point)
	{
		List<double> initial = new List<double>(){0f, 0.5f, 1f};
		double quadraticEstimate = minFuncP(point, quadraticMethod(point, initial, 4));
		//print(bezierPosition(quadraticEstimate)[0]);
		//double newtonsEstimate = newtonsMethod(point, quadraticEstimate, 5);
		return bezierPosition(quadraticEstimate);
	}
	
	public double funcD(double t, List<double> pos)
	{
		List<double> point = bezierPosition(t);
		return ((pos[0] - point[0]) * (pos[0] - point[0])) + ((pos[1] - point[1]) * (pos[1] - point[1]));
	}
	
	public double funcDY(double t, List<double> pos)
	{
		List<double> point = bezierPosition(t);
		List<double> slope = bezierSlope(t);
		return 2 * ((point[0] - pos[0]) * slope[0]) + ((point[1] - pos[1]) * slope[1]);
	}
	
	public double funcDDY(double t, List<double> pos)
	{
		List<double> point = bezierPosition(t);
		List<double> slope = bezierSlope(t);
		List<double> acceleration = bezierAcceleration(t);
		return 2 * ((slope[0] * slope[0])+(acceleration[0] * (point[0] - pos[0]))+(slope[1] * slope[1])+(acceleration[1] * (point[1] - pos[1])));
	}
	
	public double minFuncP(List<double> point, List<double> slist)
	{
		double y23 = ((slist[1] * slist[1]) - (slist[2] * slist[2])) * funcD(slist[0], point);
		double y31 = ((slist[2] * slist[2]) - (slist[0] * slist[0])) * funcD(slist[1], point);
		double y12 = ((slist[0] * slist[0]) - (slist[1] * slist[1])) * funcD(slist[2], point);
		double s23 = (slist[1] - slist[2]) * funcD(slist[0], point);
		double s31 = (slist[2] - slist[0]) * funcD(slist[1], point);
		double s12 = (slist[0] - slist[1]) * funcD(slist[2], point);
		
		return 0.5f * ((y23 + y31 + y12)/(s23 + s31 + s12));
	}
	
	public List<double> quadraticMethod(List<double> point, List<double> slist, int iterations)
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
	
	public List<double> bezierPosition(double t)
	{
		List<double> summation = new List<double>(){0,0};
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= n; i++)
		{
			summation = combine(summation, distribute((new List<double>(){controlPoints[i].position.x,controlPoints[i].position.y}), funcB(n, i, t)));
		}
		return summation;
	}
	
	public List<double> bezierSlope(double t)
	{
		List<double> summation = new List<double>(){0,0};
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= (n - 1); i++)
		{
			summation = combine(summation, distribute((new List<double>(){controlPoints[i+1].position.x-controlPoints[i].position.x,controlPoints[i+1].position.y-controlPoints[i].position.y}), (funcB(n-1, i, t) * n)));
		}
		return summation;
	}	
	
	public List<double> bezierAcceleration(double t)
	{
		List<double> summation = new List<double>(){0,0};
		int n = controlPoints.Length - 1;
		for(int i = 0; i <= (n - 2); i++)
		{
			summation = combine(summation, distribute(toList(controlPoints[i+2].position - (2 * controlPoints[i+1].position) - controlPoints[i].position), (funcB(n-2, i, t) * n * (n-1))));
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
	
	public Vector3 toVector(List<double> pos)
	{
		if(pos.Count == 2)
		{
			pos.Add(0);
		}
		return new Vector3((float)pos[0], (float)pos[1], (float)pos[2]);
	}
	
	public List<double> toList(Vector3 pos)
	{
		return new List<double>(){(double)pos.x, (double)pos.y, (double)pos.z};
	}
	
	public List<double> distribute(List<double> doubles, double magnitude)
	{
		for(int i = 0; i < doubles.Count; i++)
		{
			doubles[i] *= magnitude;
		}
		return doubles;
	}
	
	public List<double> combine(List<double> first, List<double> second)
	{
		for(int i = 0; i < second.Count; i++)
		{
			first[i] += second[i];
		}
		return first;
	}
	
	public List<double> decombine(List<double> first, List<double> second)
	{
		for(int i = 0; i < first.Count; i++)
		{
			first[i] -= second[i];
		}
		return first;
	}
}