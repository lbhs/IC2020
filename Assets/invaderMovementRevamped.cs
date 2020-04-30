using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invaderMovementRevamped : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody target;
    // public Vector3 targetVelocity;
    public Vector3 targetPosition;
    private int transitNumber;
	public static int Speedboost;
	public static int booster = 0;
	

    // Start is called before the first frame update
    void Start()
    {
        target = gameObject.GetComponent<Rigidbody>();
        targetPosition = target.position;
        //targetVelocity = target.velocity;
        transitNumber = 0;
		Speedboost = 10;
    }

    public void move()
    {
        if (transitNumber % 2 == 0)
        {
            if (Mathf.Abs(targetPosition.x - target.position.x) > 9)
            {
                moveDown();
            }
            else
            {
                target.velocity = new Vector3(Speedboost, 0, 0);
            }
        }
        else
        {
            if (Mathf.Abs(targetPosition.x - target.position.x) > 9)
            {
                moveDown();
            }
            else
            {
                target.velocity = new Vector3((Speedboost * -1), 0, 0);
            }
        }
    }

    public void moveDown()
    {
        if (Mathf.Abs(targetPosition.y - target.position.y) < 1)
        {
            target.velocity = new Vector3(0, -10, 0);
        }
        else
        {
            target.velocity = new Vector3(0, 0, 0);
            targetPosition = target.position;
            transitNumber += 1;
        }
    }
	public void movespeed()
	{
		
		Speedboost = Speedboost + 1;
		
	}
    // Update is called once per frame
    void Update()
    {
        move();
		
    }
}
