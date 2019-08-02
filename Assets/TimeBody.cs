using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
	private List<SpaceTime> points;
    public Rigidbody Arbies;
    public GameObject McDonalds;
	private bool rewind = false;
    
	public static bool isRewinding
	{
		get
		{
			return rewind;
		}
		set
		{
			rewind = value;
			if (rewind)
			{
				Arbies.isKinematic = true;
			}
			else
			{
				SpaceTime point = points[0];
				points.RemoveAt(0);
				Arbies.isKinematic = false;
				Arbies.velocity = point.velocity;
			}
		}
	}
	
    void Start()
    {
        points = new List<SpaceTime>();
        Arbies = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rewind)
            Rewind();
        else
            Record();
    }

    void Rewind()
    {
		GetComponent<forces>().frameNumber--;
        if (points.Count == 0)
        {
            isRewinding = false;
            //Destroy(McDonalds);
        }
        else
        {
            SpaceTime point = points[0];
            transform.position = point.position;
            transform.rotation = point.rotation;
            points.RemoveAt(0);
        }
    }
    
    void Record()
    {   
        points[GameObject.Find("GameObject").GetComponent<forces>().frameNumber] = new SpaceTime(Arbies.velocity, transform.position, transform.rotation);
    }
}