using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour {

	Rigidbody rb;
	public Vector3 diceVelocity;
	private Vector3 startPos;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
	}
	
	void Update()
	{
		diceVelocity = rb.velocity;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			DiceNumberTextScript.diceNumber1 = 0;
			DiceNumberTextScript.diceNumber2 = 0;
			rb.velocity = Vector3.zero;
			float dirX = Random.Range(-500, 500);
			float dirY = Random.Range(-500, 500);
			float dirZ = Random.Range(-500, 500);
			transform.position = startPos;
			transform.rotation = Random.rotation;
			rb.AddForce(new Vector3(Random.Range(-100f, 100f), 1500, 0));
			rb.AddTorque(dirX, dirY, dirZ);
		}
	}
	
	public void Reset()
	{
		transform.position = startPos;
	}
}
