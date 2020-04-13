using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnCheck : MonoBehaviour
{
	[SerializeField]
	public GameObject GameSetup;
	public GameObject die1;
	public GameObject die2;

    public void rollDie()
	{
		Debug.Log(GameSetup.GetComponent<GameSetupContrller>().state);
		
		if(GameSetup.GetComponent<GameSetupContrller>().state == GameState.Player1Turn)
		{
			Debug.Log("roll die 1");
			die1.GetComponent<DieScript>().RollDiceAnimation();
		}
		else if(GameSetup.GetComponent<GameSetupContrller>().state == GameState.Player2Turn)
		{
			Debug.Log("roll die 2");
			die2.GetComponent<OwnershipTransfer>().steal();
			die2.GetComponent<DieScript>().RollDiceAnimation();
		}
	}
}
