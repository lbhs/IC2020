using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public bool isRewinding = false;


    private List<SpaceTime> points;

    public Rigidbody Arbies;
    public GameObject McDonalds;
    
    
    
    void Start()
    {
        points = new List<SpaceTime>();
        Arbies = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.Space))
            StopRewind();
    }

    void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
        
    }

    void Rewind()
    {
        if (points.Count == 0)
        {
            StopRewind();
            //Destroy(McDonalds);
        }
        else
        {
            SpaceTime point = points[0];
            SpaceTime vpoint = points[1];
            transform.position = point.position;
            transform.rotation = point.rotation;
            points.RemoveAt(0);
        }



    }
    
    void Record()
    {
        if (points.Count > Mathf.Round(180f/Time.fixedDeltaTime))
        {
            points.RemoveAt(points.Count - 1);
        }
        
        points.Insert(0, new SpaceTime(Arbies.velocity, transform.position, transform.rotation));
        Debug.Log("Recording" + Arbies.velocity);
    }

    public void StartRewind()
    {
        isRewinding = true;
        Arbies.isKinematic = true;
    }
    
    public void StopRewind()
    {
        SpaceTime point = points[0];
        points.RemoveAt(0);
        isRewinding = false;
        Arbies.isKinematic = false;
        Arbies.velocity = point.velocity;
        Debug.Log("Applying" + Arbies.velocity);
    }
}
 