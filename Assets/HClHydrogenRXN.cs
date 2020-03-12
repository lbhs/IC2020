﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HClHydrogenRXN : MonoBehaviour
{
	public GameObject HCl;
	public GameObject Chlorine;
	public GameObject ClTwo;
	private Vector3 ColPosition;
	//private HClChlorineMolecule HClChlorineMolecule;
	private Rigidbody HAtom;
    // Start is called before the first frame update
    void Start()
    {
        HAtom = gameObject.GetComponent<Rigidbody>();
        //gameObject.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	void OnCollisionEnter(Collision collider)
	{
		if(collider.gameObject.tag == "Cl atom")
		{
			ColPosition = collider.gameObject.transform.position;

			Destroy(gameObject);
			Destroy(collider.gameObject);
			//Instantiate(HCl, ColPosition, Quaternion.identity);
			Instantiate(HCl, new Vector3(ColPosition.x + 1, ColPosition.y, 0), Quaternion.identity);
		}
		if(collider.gameObject.tag == "ChlorineMol")
		{
			ColPosition = collider.gameObject.transform.position;
			
			Destroy(gameObject);
			Destroy(collider.gameObject);
			Instantiate(HCl, new Vector3(ColPosition.x + 1, ColPosition.y, 0), Quaternion.identity);
			Instantiate(Chlorine, new Vector3(ColPosition.x - 1, ColPosition.y, 0), Quaternion.identity);
			Instantiate(ClTwo, new Vector3(-1f, 5.2f, 0), Quaternion.identity);
		}
	}
}

