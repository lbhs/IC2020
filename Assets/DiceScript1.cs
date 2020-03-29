using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript1 : MonoBehaviour {

	static Rigidbody rb;
	public static Vector3 diceVelocity;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
		diceVelocity = rb.velocity;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			DiceNumberTextScript.diceNumber1 = 0;
			float dirX = Random.Range(0, 500);
			float dirY = Random.Range(0, 500);
			float dirZ = Random.Range(0, 500);
			transform.position = new Vector3(1, -9, -3);
			transform.rotation = Quaternion.identity;
			rb.AddForce(transform.up * 2000);
			rb.AddTorque(dirX, dirY, dirZ);
		}
	}
}
