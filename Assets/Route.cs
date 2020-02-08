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
			Gizmos.DrawSphere(bezierPosition(t), 0.25f);
		}
	}
	
	private void Start()
	{
		//draw visual representation of the bezier curve in runtime
		Vector3 previousPos = bezierPosition(0f);
		for (float t = 0.025f; t <= 1; t += 0.025f)
		{
			Vector3 currentPos = bezierPosition(t);
			DrawLine(previousPos, currentPos, Color.black);
			previousPos = currentPos;
		}
		DrawLine(previousPos, bezierPosition(1f), Color.black);
	}
	
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
	
	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 10000f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Standard"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}


