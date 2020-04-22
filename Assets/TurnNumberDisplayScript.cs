using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnNumberDisplayScript : MonoBehaviour
{
    public Text TurnNumber;

    // Update is called once per frame
    void Update()
    {
		if(DieScript.totalRolls == 12)
		{
			TurnNumber.text = "Final Turn!\nRoll again to end the game."; //the "\n" forces it to go to the next line
		}
		else
		{
			TurnNumber.text = "Turn Number " + DieScript.totalRolls;
		}
    }
}
