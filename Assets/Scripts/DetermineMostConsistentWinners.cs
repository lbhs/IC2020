using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PlayerNameAndScore
{
    public string Username;
    public int Score;
}

public class DetermineMostConsistentWinners : MonoBehaviour
{
    private string CSVUrl;
    private List<PlayerNameAndScore> leaderboardEntries;

    #region Programmatic Layout
    [SerializeField]
    private GameObject LeaderboardParent;

    [SerializeField]
    private float startingPanelX;

    [SerializeField]
    private float startingPanelY;

    [SerializeField]
    private float YDecrement;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // CSVUrl locates an automatically-updated CSV file with player scores
        CSVUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRZcX-j72BBigQmH12yg6r_WH1yi0yT78RljvmWZi29oGZWouLwOZUUPtSy-HEiqs99eXN8Oyry9aJi/pub?output=csv";
        leaderboardEntries = new List<PlayerNameAndScore>();
        // Only needs to execute once: GameObjects will persist for the life of the scene
        StartCoroutine(GetDataFromURL());
    }

    public IEnumerator GetDataFromURL()
    {
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

        string[] results = www.text.Split('\n');

        foreach (string record in results.Skip(1))
        {
            string[] recordProperties = record.Split(',');

            PlayerNameAndScore player1Instance = new PlayerNameAndScore();
            PlayerNameAndScore player2Instance = new PlayerNameAndScore();

            player1Instance.Username = recordProperties[1];
            player1Instance.Score = int.Parse(recordProperties[3]);
            leaderboardEntries.Add(player1Instance);

            player2Instance.Username = recordProperties[2];
            player2Instance.Score = int.Parse(recordProperties[4]);
            leaderboardEntries.Add(player2Instance);
        }

        leaderboardEntries = leaderboardEntries.OrderByDescending(i => i.Score).ToList();

        int rowCount = 0;
        // There may be less than 20 entries in the leaderboard
        while (rowCount < 20 && rowCount < leaderboardEntries.Count)
        {
            GameObject GO = new GameObject();
            GO.transform.SetParent(LeaderboardParent.transform);
            // UI layer
            GO.layer = 5;

            GO.AddComponent<RectTransform>();
            GO.transform.localPosition = new Vector3(startingPanelX, startingPanelY - rowCount * YDecrement);
            GO.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 39);

            Text newText = GO.AddComponent<Text>();
            newText.color = Color.red;
            newText.text = string.Format("{0}: {1}", leaderboardEntries[rowCount].Username, leaderboardEntries[rowCount].Score);
            newText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            newText.alignment = TextAnchor.MiddleCenter;

            rowCount++;
        }
    }
}
