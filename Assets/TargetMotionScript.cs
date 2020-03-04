using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMotionScript : MonoBehaviour
{
    public Rigidbody target;
    public Vector3 targetVelocity;
    private Vector3 positionOnCollision;
    private int TransitNumber;

    

    // Start is called before the first frame update
    void Start()
    {
    target = gameObject.GetComponent<Rigidbody>();
    target.velocity = targetVelocity;  
    TransitNumber = 1;  
    
    }

    private void OnCollisionEnter(Collision collider)
    {
        print(collider.gameObject.tag);

        if (collider.gameObject.tag == "wall")
        {
        
        target.velocity = targetVelocity;
        target.transform.position = new Vector3(-8.0f, 5.2f, 0);
        //TransitNumber ++;
        //print(TransitNumber);

        
        }

        /*if(TransitNumber == 4)  //SpaceInvader will only make 3 transits before disappearing
        {
            //target.gameObject.GetComponent<Collider>().enabled = false;  //can now pass through the right hand wall
            target.velocity = targetVelocity;
            target.transform.position = new Vector3(-8.5f, 6.8f, 0);
            TransitNumber = 1;
            //Instantiate(target, new Vector3(-8.0f,5.2f,0f), Quaternion.identity);            
            //target.transform.position = new Vector3(50, 50, 50);
        }
        
        else  //SpaceInvader will collide, reverse velocity and move drop down one "row" (closer to player's base)
        {
            positionOnCollision = target.transform.position;
            print("position" + positionOnCollision);
            target.transform.position = new Vector3(positionOnCollision.x, positionOnCollision.y-2, 0);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*if(gameObject.transform.position.x > 13.5)
        {                  
            target.velocity = targetVelocity;
            target.transform.position = new Vector3(-7.0f, 5.2f, 0);
        }*/
    }
}
