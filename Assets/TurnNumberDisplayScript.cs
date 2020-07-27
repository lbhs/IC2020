using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnNumberDisplayScript : MonoBehaviour
{
    public Text TurnNumber;
	private int rolls = 0;

    // Update is called once per frame
    void Update()
    {
		if(rolls != DieScript.totalRolls)
		{
			rolls = DieScript.totalRolls;
			if(DieScript.totalRolls == 12)
			{
				GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().finalTurn();
			}
			TurnNumber.text = "Turn Number = " + DieScript.totalRolls;
		}
		
    }
}