using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhotonFire : MonoBehaviour

{
	//public Rigidbody photon;
	public GameObject photon;
	public GameObject bluephoton;
	public GameObject uvphoton;
	private photonParts photonParts;
	public static int timer = 0;
	int color = 1;
    // Start is called before the first frame update
    void Start()
    {
        photonParts = GameObject.Find("cannon").GetComponent<photonParts>();
		//photon = gameObject.GetComponent<Rigidbody>();
	    
    }

    // Update is called once per frame
    void Update()
    {
		timer--;
        if(Input.GetKeyDown("down"))
		{
			if(color <= 2)
			{
				color = color + 1;
			}
			else
			{
				color = 1;
			}
		}
		if(Input.GetKeyDown("up") && timer <= 0)
		{
			timer = 30;
			//Rigidbody photonInstance;
			//photonInstance = 
			if(color == 1)
			{
			Instantiate(photon, transform.position, Quaternion.identity);//.GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
			}
			if(color == 2)
			{
			Instantiate(bluephoton, transform.position, Quaternion.identity);
			}
			if(color == 3) 
			{
			Instantiate(uvphoton, transform.position, Quaternion.identity);
			}
				
	}
		
	}
	
}
