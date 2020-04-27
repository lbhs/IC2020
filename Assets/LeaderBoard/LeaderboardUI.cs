using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject InputFeildUI;
    public InputField IFEmail;
    public GameOver GameOverScript;
    public LeaderboardScript LBS;
    public GameObject errorText;
    public GameObject successText;

    public void LBSendButtonFunction()
    {
        if (IFEmail.text != null)
        {
            string n = IFEmail.text;
            int s = GameOverScript.FinalGameOverScore;
            bool b = LBS.Send(n, s);
            if (b == false)
            {
                errorText.SetActive(true);
            }
            else
            {
                successText.SetActive(true);
                InputFeildUI.SetActive(false);
            }
        }
        else
        {
            errorText.SetActive(true);
        }
    }
}
