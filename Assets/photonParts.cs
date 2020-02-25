using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonParts : MonoBehaviour
{
	public Rigidbody photon;
	public Rigidbody bluephoton;
	public Rigidbody uvphoton;
	public Vector3 velocity ;
	
    // Start is called before the first frame update
    void Start()
    {
       // photonSpawn = GameObject.GetComponent<Rigidbody>();
	   photon = gameObject.GetComponent<Rigidbody>();
	   bluephoton = gameObject.GetComponent<Rigidbody>();
	   uvphoton = gameObject.GetComponent<Rigidbody>();
	   photon.isKinematic = false;
	   bluephoton.isKinematic = false;
	   
	   photon.velocity = velocity;
	   bluephoton.velocity = velocity;
	   uvphoton.velocity = velocity;
	   //Destroy(gameObject, 2);
	   
    }

    // Update is called once per frame
    void Update()
    {
        
		
		
	
    }
	void OnCollisionEnter (Collision collision)
	{
		Destroy(gameObject);
		
	}
}
