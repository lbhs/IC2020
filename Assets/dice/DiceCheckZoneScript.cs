using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript : MonoBehaviour {
	
	[SerializeField]
	public GameObject die1;
	public GameObject die2;
	public GameObject GameSetup;

	Vector3 diceVelocity;
	Vector3 diceVelocity2;
	
	public static int diceNumber1;
	public static int diceNumber2;

	// Update is called once per frame
	void FixedUpdate () {
		diceVelocity = die1.GetComponent<DiceScript>().diceVelocity;
		diceVelocity2 = die2.GetComponent<DiceScript>().diceVelocity;
		
		if(DiceScript.rolling == 2 && diceNumber1 != 0 && diceNumber2 != 0)
		{
			//send the outcome of the roll to the main program
			Debug.Log("roll: " + (diceNumber1 + diceNumber2).ToString());
			GameSetup.GetComponent<GameSetupContrller>().RollDice(diceNumber1 + diceNumber2);
			
			diceNumber1 = 0;
			diceNumber2 = 0;
			
			DiceScript.rolling = 3;
			
			die1.GetComponent<fade>().startFade();
			die2.GetComponent<fade>().startFade();
		}
	}

	void OnTriggerStay(Collider col)
	{
		if ((diceVelocity == Vector3.zero) && (diceVelocity2 == Vector3.zero) && die1.transform.position.y > -3 && (DiceScript.rolling == 2))
		{
			switch (col.gameObject.name)
			{
				case "1Side1":
					diceNumber1 = 6;
					break;
				case "1Side2":
					diceNumber1 = 5;
					break;
				case "1Side3":
					diceNumber1 = 4;
					break;
				case "1Side4":
					diceNumber1 = 3;
					break;
				case "1Side5":
					diceNumber1 = 2;
					break;
				case "1Side6":
					diceNumber1 = 1;
					break;
				case "2Side1":
					diceNumber2 = 6;
					break;
				case "2Side2":
					diceNumber2 = 5;
					break;
				case "2Side3":
					diceNumber2 = 4;
					break;
				case "2Side4":
					diceNumber2 = 3;
					break;
				case "2Side5":
					diceNumber2 = 2;
					break;
				case "2Side6":
					diceNumber2 = 1;
					break;
			}
		}
	}
}
