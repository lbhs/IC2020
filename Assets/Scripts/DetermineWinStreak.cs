using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

//[Serializable]
//public class SerializedScoreboardData
//{
//    public string[] CSVSplitByLines;
//}

public class DetermineWinStreak : MonoBehaviour
{
    private string CSVUrl;

    [SerializeField]
    private Text StreakTextField;

    [SerializeField]
    private GameObject LeaderboardUpdateWarnMessage;

    // Start is called before the first frame update
    void Start()
    {
        // CSVUrl locates an automatically-updated CSV file with player scores
        CSVUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRZcX-j72BBigQmH12yg6r_WH1yi0yT78RljvmWZi29oGZWouLwOZUUPtSy-HEiqs99eXN8Oyry9aJi/pub?output=csv";
    }

    public IEnumerator GetDataFromURL()
    {
        int wins = 0;

        // If we have not already serialized the CSV file (split by lines)
        //if (PlayerPrefs.GetString("PlayerScoreData") == null)
        //{
        //    WWW www = new WWW(url);

        //    // Asynchronous call
        //    yield return www;

        //    SerializedScoreboardData serializedScoreboardData = new SerializedScoreboardData();

        //    // Each row is separated with a newline
        //    serializedScoreboardData.CSVSplitByLines = www.text.Split('\n');

        //    PlayerPrefs.SetString("PlayerScoreData", JsonUtility.ToJson(serializedScoreboardData));
        //}

        //string[] results = JsonUtility.FromJson<SerializedScoreboardData>(PlayerPrefs.GetString("PlayerScoreData")).CSVSplitByLines;

        WWW www = new WWW(CSVUrl);

        yield return www;

        Debug.Log(www.text);

        string[] results = www.text.Split('\n');

        foreach (string record in results.Skip(1))
        {
            string[] recordProperties = record.Split(',');

            // Case-insensitive search retrieval
            recordProperties[1] = recordProperties[1].ToLower();
            recordProperties[2] = recordProperties[2].ToLower();
            StartLobbyController.FinalUsername = StartLobbyController.FinalUsername.ToLower();

            if (recordProperties.Contains(StartLobbyController.FinalUsername))
            {
                int thisPlayerScore;
                int opponentScore;

                // In this match, was Player 1 the player identified by StartLobbyController.FinalUsername?
                if (Array.IndexOf(recordProperties, StartLobbyController.FinalUsername) == 1)
                {
                    thisPlayerScore = int.Parse(recordProperties[3]);
                    opponentScore = int.Parse(recordProperties[4]);
                }
                else
                {
                    thisPlayerScore = int.Parse(recordProperties[4]);
                    opponentScore = int.Parse(recordProperties[3]);
                }

                if (thisPlayerScore > opponentScore)
                {
                    wins++;
                }
            }
        }

        if (wins > 0)
        {
            StreakTextField.text = string.Format("Congratulations on {0} wins!", wins);
        }
        else
        {
            StreakTextField.text = "You do not have any wins";
            LeaderboardUpdateWarnMessage.SetActive(true);
        }
    }
}
