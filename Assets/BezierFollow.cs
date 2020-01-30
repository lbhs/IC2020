using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
	[SerializeField]
	private Transform route;

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
	
	private Vector3 bezierPosition(float t)
	{
		Vector2 p0 = route.GetChild(0).position;
		Vector2 p1 = route.GetChild(1).position;
		Vector2 p2 = route.GetChild(2).position;
		Vector2 p3 = route.GetChild(3).position;
		return Mathf.Pow(1 - t, 3) * p0 + 3* Mathf.Pow(1 - t, 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
	}
	
	
}
