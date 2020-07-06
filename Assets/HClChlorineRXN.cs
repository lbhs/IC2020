using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HClChlorineRXN : MonoBehaviour
{
	public GameObject HCl;
	public GameObject Hydrogen;
	public GameObject HTwo;
	public photonParts photonParts;
	private Vector3 ColPosition1;
	private Vector3 ColPosition;
	private Rigidbody ClAtom;
	public Scoreboard scoreboard;
	
    // Start is called before the first frame update
    void Start()
    {
        ClAtom = gameObject.GetComponent<Rigidbody>();
		
    }

    // Update is called once per frame
    void Update()
    {
       
    }
	void OnCollisionEnter(Collision collider)
	{
	
		if(collider.gameObject.tag == "HMol")
		{
			scoreboard.UpdateScore();
			scoreboard.MultiUp();
			ColPosition = collider.gameObject.transform.position;
			ColPosition1 = photonParts.getPosition(ColPosition1);
			Destroy(gameObject);
			//Destroy(collider.gameObject);
			Instantiate(HCl, new Vector3(ColPosition.x + 1, ColPosition.y, 0), Quaternion.identity);
			Instantiate(Hydrogen, new Vector3(ColPosition.x, ColPosition.y, 0), Quaternion.identity);
			//collider.gameObject.transform.position = new Vector3(Random.Range(-8f, 12f), Random.Range(1f, 5.3f),0);
			collider.gameObject.transform.position = ColPosition1;

		}

	}
}
