using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript : MonoBehaviour {
	
	[SerializeField]
	public GameObject die1;
	public GameObject die2;

	Vector3 diceVelocity;
	Vector3 diceVelocity2;

	// Update is called once per frame
	void FixedUpdate () {
		diceVelocity = die1.GetComponent<DiceScript>().diceVelocity;
		diceVelocity2 = die2.GetComponent<DiceScript>().diceVelocity;
	}

	void OnTriggerStay(Collider col)
	{
		if ((diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f) && (diceVelocity2.x == 0f && diceVelocity2.y == 0f && diceVelocity2.z == 0f))
		{
			switch (col.gameObject.name) {
			case "1Side1":
				DiceNumberTextScript.diceNumber1 = 6;
				break;
			case "1Side2":
				DiceNumberTextScript.diceNumber1 = 5;
				break;
			case "1Side3":
				DiceNumberTextScript.diceNumber1 = 4;
				break;
			case "1Side4":
				DiceNumberTextScript.diceNumber1 = 3;
				break;
			case "1Side5":
				DiceNumberTextScript.diceNumber1 = 2;
				break;
			case "1Side6":
				DiceNumberTextScript.diceNumber1 = 1;
				break;
			}
			
			switch (col.gameObject.name) {
			case "2Side1":
				DiceNumberTextScript.diceNumber2 = 6;
				break;
			case "2Side2":
				DiceNumberTextScript.diceNumber2 = 5;
				break;
			case "2Side3":
				DiceNumberTextScript.diceNumber2 = 4;
				break;
			case "2Side4":
				DiceNumberTextScript.diceNumber2 = 3;
				break;
			case "2Side5":
				DiceNumberTextScript.diceNumber2 = 2;
				break;
			case "2Side6":
				DiceNumberTextScript.diceNumber2 = 1;
				break;
			}
		}
	}
}
