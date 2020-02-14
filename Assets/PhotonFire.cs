using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhotonFire : MonoBehaviour

{
	//public Rigidbody photon;
	public GameObject photon;
	private photonParts photonParts;
    // Start is called before the first frame update
    void Start()
    {
        photonParts = GameObject.Find("cannon").GetComponent<photonParts>();
		//photon = gameObject.GetComponent<Rigidbody>();
	    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("up"))
		{
			//Rigidbody photonInstance;
			//photonInstance = 
			Instantiate(photon, transform.position, Quaternion.identity);//.GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
			
		
	}
		
	}
}
