using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public bool isRewinding = false;
	private int frame = 0;

    private List<SpaceTime> points;

    public Rigidbody Arbies;
    public GameObject McDonalds;
        
    void Start()
    {
        points = new List<SpaceTime>();
        Arbies = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isRewinding)
			frame--;
            Rewind();
        else if(#ARRAY#[frameNumber] != null)
			frame++;
			Playback();
		else
			frame++;
			Record();
    }

    void Rewind()
    {
        if (frame == 0)
        {
            StopRewind();
        }
        else
        {
            SpaceTime point = points[frame];
            transform.position = point.position;
            transform.rotation = point.rotation;
        }
    }
    
    void Record()
    {
		points[frame] = new SpaceTime(Arbies.velocity, transform.position, transform.rotation);
    }
	
	void Playback()
	{
		
	}

    public void StartRewind()
    {
        isRewinding = true;
        Arbies.isKinematic = true;
    }
	
	public void StartPlayback()
	{
		
	}
    
    public void StopRewind()
    {
        isRewinding = false;
    }
}