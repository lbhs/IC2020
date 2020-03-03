using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Davidsthing : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody target;
   // public Vector3 targetVelocity;
    public Vector3 targetPosition;
    private int transitNumber;



    // Start is called before the first frame update
    void Start()
    {
        target = gameObject.GetComponent<Rigidbody>();
        targetPosition = target.position;
        //targetVelocity = target.velocity;
        transitNumber = 0;

    }

    public void move()
    {
        if(transitNumber % 2 == 0)
        {
            if(Mathf.Abs(targetPosition.x - target.position.x) > 9)
            {
                moveDown();
            } else
            {
                target.velocity = new Vector3(2, 0, 0);
            }
        } else
        {
            if (Mathf.Abs(targetPosition.x - target.position.x) > 9)
            {
                moveDown();
            } else
            {
                target.velocity = new Vector3(-2, 0, 0);
            }
        }
    }

    public void moveDown()
    {
        if (Mathf.Abs(targetPosition.y - target.position.y) < 1)
        {
            target.velocity = new Vector3(0, -1, 0);
        } else
        {
            target.velocity = new Vector3(0, 0, 0);
            targetPosition = target.position;
            transitNumber += 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        move();
    }
}



