using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HClHydrogenRXN : MonoBehaviour
{
	public GameObject HCl;
	public GameObject Chlorine;
	public GameObject ClTwo;
	private Vector3 ColPosition;
	private Vector3 ColPosition1;
	public photonParts photonParts;
	//private HClChlorineMolecule HClChlorineMolecule;
	private Rigidbody HAtom;
	public Scoreboard scoreboard;
    // Start is called before the first frame update
    void Start()
    {
        HAtom = gameObject.GetComponent<Rigidbody>();

   
    }

    // Update is called once per frame
    void Update()
    {
       
    }
	void OnCollisionEnter(Collision collider)
	{
		
		if(collider.gameObject.tag == "Cl atom")
		{
			scoreboard.UpdateScore();
			ColPosition = collider.gameObject.transform.position;
			scoreboard.MultiUp();
			Destroy(gameObject);
			Destroy(collider.gameObject);
			
			//Instantiate(HCl, ColPosition, Quaternion.identity);
			Instantiate(HCl, new Vector3(ColPosition.x + 1, ColPosition.y, 0), Quaternion.identity);
		}
		if(collider.gameObject.tag == "ChlorineMol")
		{
			scoreboard.UpdateScore();
			ColPosition = collider.gameObject.transform.position;
            ColPosition1 = photonParts.getPosition(ColPosition1);
			scoreboard.MultiUp();
			Destroy(gameObject);
			//Destroy(collider.gameObject);
			Instantiate(HCl, new Vector3(ColPosition.x + 1, ColPosition.y, 0), Quaternion.identity);
			Instantiate(Chlorine, new Vector3(ColPosition.x - 1, ColPosition.y, 0), Quaternion.identity);
			//collider.gameObject.transform.position = new Vector3(Random.Range(-8f, 12f), Random.Range(1f, 5.3f),0);
			collider.gameObject.transform.position = ColPosition1;

		}

	}
}

