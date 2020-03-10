using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonParts : MonoBehaviour
{
	public Rigidbody photon;
	public Rigidbody bluephoton;
	public Rigidbody uvphoton;
	public Vector3 velocity;
	private Vector3 ColPosition;
	public GameObject DissociationProduct;
	public GameObject DissociationProduct2;
	public GameObject TargetMol;
	public GameObject TargetMol2;
	
	
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
	void OnCollisionEnter (Collision collider)
	{
		print(collider.gameObject.tag);

		print("photon color "+gameObject.tag);

		if(collider.gameObject.tag == "wall")
		{
			Destroy(gameObject);			
		}
		
		if((gameObject.tag == "blue" || gameObject.tag == "UV") && collider.gameObject.tag == "ChlorineMol")
		{
			print("hit the target");
			ColPosition = collider.gameObject.transform.position;

			Destroy(gameObject);
			Destroy(collider.gameObject);
			Instantiate(DissociationProduct,ColPosition,Quaternion.identity);
			Instantiate(DissociationProduct,new Vector3(ColPosition.x+1,ColPosition.y,0),Quaternion.identity);
			Instantiate(TargetMol,new Vector3(-8.0f, 5.2f,0),Quaternion.identity);
			
		}
		
		if(gameObject.tag == "UV" && collider.gameObject.tag == "HMol")
		{
			print("hit the target");
			ColPosition = collider.gameObject.transform.position;

			Destroy(gameObject);
			Destroy(collider.gameObject);
			Instantiate(DissociationProduct2,ColPosition,Quaternion.identity);
			Instantiate(DissociationProduct2,new Vector3(ColPosition.x+1,ColPosition.y,0),Quaternion.identity);
			Instantiate(TargetMol2,new Vector3(-8.0f, 4f,0),Quaternion.identity);
			
		}
		//Destroy(gameObject);
		//if(collider.gameObject.tag == "H Atom" || collider.gameObject.tag == "Cl atom")
		//{
		//	Physics.IgnoreCollision(, collider);
		//}
	}
}
