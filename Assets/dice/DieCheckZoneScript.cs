using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCheckZoneScript : MonoBehaviour {
	
	[SerializeField]
	public GameObject die;
	public GameObject GameSetup;

	Vector3 dieVelocity;
	
	public int dieNumber;

	// Update is called once per frame
	void FixedUpdate () {
		dieVelocity = die.GetComponent<DieScript>().dieVelocity;
		if(die.GetComponent<DieScript>().rolling == 1 && dieNumber != 0)
		{
			//send the outcome of the roll to the main program
			Debug.Log("roll: " + dieNumber.ToString());
			GameSetup.GetComponent<GameSetupContrller>().RollDice(dieNumber);
			dieNumber = 0;
			die.GetComponent<DieScript>().rolling = 2;
			die.GetComponent<DieFade>().startFade();
		}
	}

	void OnTriggerStay(Collider col)
	{
		if(((dieVelocity == Vector3.zero) && die.transform.position.y > 0 && (die.GetComponent<DieScript>().rolling == 1) && die.transform.parent.name == "die_player1") || ((dieVelocity == Vector3.zero) && die.transform.position.y > -16 && (die.GetComponent<DieScript>().rolling == 1) && die.transform.parent.name == "die_player2"))
		{
			switch(col.gameObject.name)
			{
				case "Side1":
					dieNumber = 6;
					break;
				case "Side2":
                    dieNumber = 5;
                    break;
				case "Side3":
                    dieNumber = 4;
                    break;
				case "Side4":
                    dieNumber = 3;
                    break;
				case "Side5":
                    dieNumber = 2;
                    break;
				case "Side6":
                    dieNumber = 1;
                    break;
			}
		}
	}
}
