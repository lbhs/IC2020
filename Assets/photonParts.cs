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
	private Vector3 ColPosition1;
	public GameObject DissociationProduct;
	public GameObject DissociationProduct2;
	public GameObject TargetMol;
	public GameObject TargetMol2;
	public Scoreboard scoreboard;
	
	
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
    public Vector3 getPosition(Vector3 samplePosition)
    {
		Vector3 pos1 = new Vector3(-8.04f, 6, 0);
		Vector3 pos2 = new Vector3(-5.67f, 4, 0);
		Vector3 pos3 = new Vector3(-0.6755303f, 6, 0);
		Vector3 pos4 = new Vector3(-3.72f, 4, 0);
		int selection = Random.Range(1, 5);
        switch(selection)
        {
            case 1:
				samplePosition = pos1;
				break;
			case 2:
				samplePosition = pos2;
				break;
			case 3:
				samplePosition = pos3;
				break;
			case 4:
				samplePosition = pos4;
				break;
        }
		return samplePosition;

	}
	void OnCollisionEnter (Collision collider)
	{
		print(collider.gameObject.tag);

		print("photon color "+gameObject.tag);

		if(collider.gameObject.tag == "wall")
		{
			Destroy(gameObject);			
			scoreboard.MultiDown();
		}
		
		if((gameObject.tag == "blue" || gameObject.tag == "UV") && collider.gameObject.tag == "ChlorineMol")
		{
			print("hit the target");
            ColPosition = collider.gameObject.transform.position;
            ColPosition1 = getPosition(ColPosition1);
			print(ColPosition);
			scoreboard.MultiUp();
			Destroy(gameObject);
			//Destroy(collider.gameObject);
			Instantiate(DissociationProduct,ColPosition,Quaternion.identity);
			Instantiate(DissociationProduct,new Vector3(ColPosition.x+1,ColPosition.y,0),Quaternion.identity);
			//collider.gameObject.transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(1f, 5.3f),0);
			collider.gameObject.transform.position = ColPosition1;
			print(collider.gameObject.transform.position);

		}

		if (gameObject.tag == "UV" && collider.gameObject.tag == "HMol")
		{
			print("hit the target");
			ColPosition = collider.gameObject.transform.position;
			ColPosition1 = getPosition(ColPosition1);
			scoreboard.MultiUp();
			Destroy(gameObject);
			//Destroy(collider.gameObject);
			Instantiate(DissociationProduct2,ColPosition,Quaternion.identity);
			Instantiate(DissociationProduct2,new Vector3(ColPosition.x+1,ColPosition.y,0),Quaternion.identity);
			//collider.gameObject.transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(1f, 5.3f),0);
			collider.gameObject.transform.position = ColPosition1;
			print(collider.gameObject.transform.position);


		}
		//if(gameObject.tag == "UV" && collider.gameObject.tag == "glass")
		//{
		//print("hit glass");
		//Destroy(gameObject);
		//}
		//Destroy(gameObject);
		//if(collider.gameObject.tag == "H Atom" || collider.gameObject.tag == "Cl atom")
		//{
		//	Physics.IgnoreCollision(, collider);
		//}
	}
}
