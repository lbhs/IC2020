using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCheckZoneScript : MonoBehaviour {
	
	[SerializeField]
	public GameObject die;
	public GameObject GameSetup;

	Vector3 dieVelocity;
	
	public static int dieNumber;

	// Update is called once per frame
	void FixedUpdate () {
		dieVelocity = die.GetComponent<DieScript>().dieVelocity;
		
		if(DieScript.rolling == 1 && dieNumber != 0)
		{
			//send the outcome of the roll to the main program
			Debug.Log("roll: " + dieNumber.ToString());
			GameSetup.GetComponent<GameSetupContrller>().RollDice(dieNumber);
			dieNumber = 0;
			DieScript.rolling = 2;
			die.GetComponent<DieFade>().startFade();
		}
	}

	void OnTriggerStay(Collider col)
	{
		if ((dieVelocity == Vector3.zero) && die.transform.position.y > -3 && (DieScript.rolling == 1))
		{
			switch (col.gameObject.name)
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
