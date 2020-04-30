using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SimpleJSON;
#pragma warning disable CS0618 // Type or member is obsolete
public class Player
{
    public string Name;
    public int Score;
}

public struct Team
{
    public string TeamName;
    public string P1, P2, P3;
    public int Score;
}

public class UpdateLeaderboard : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public List<Team> teams = new List<Team>();
    public GameObject[] textBoxes;

    private void OnEnable()
    {
        clear();
        StartCoroutine(GetString());
        // Google Sheet ID: "163XTG4mOdzoAZv-HHcKGoRtDum-XvTOsajDYoiO97VA" Sub Sheet name: "Leaderboard")
    }

    IEnumerator GetString()
    {
        string url = "https://sheets.googleapis.com/v4/spreadsheets/163XTG4mOdzoAZv-HHcKGoRtDum-XvTOsajDYoiO97VA/values/Leaderboard!" + "B2" + ":" + "G1001" + "?key=" + SecretKey.GSkey;
        WWW www = new WWW(url);
        yield return www;
        string RecivedJSON;
        RecivedJSON = www.text;
        var J = JSON.Parse(RecivedJSON);
        Debug.Log(J);
        for (int i = 2; i < 1001; i++)
        {
            Player p = new Player();
            string n = J["values"][i][0].Value;
            p.Name = n;
            string s = J["values"][i][1].Value;
            int a;
            if (int.TryParse(s, out a))
            {
                p.Score = int.Parse(s);
                players.Add(p);
            }
        }
        players = players.OrderByDescending(i => i.Score).ToList();

        for (int i = 2; i < 24; i++)
        {
            Team t = new Team();
        }

        applyUpdates();
    }

    void applyUpdates()
    {

       
        //solitaire version code:

        int num = 0;
        foreach (var item in textBoxes)
        {
            item.transform.GetChild(0).GetComponent<Text>().text = num + 1 + ":";
            item.transform.GetChild(1).GetComponent<Text>().text = players[num].Name;
            item.transform.GetChild(2).GetComponent<Text>().text = players[num].Score.ToString();
            num++;
        }
        //Debug.Log(players[players.Count-1].Score);
    }

    void clear()
    {
        players.Clear();
        int i = 1;
        foreach (var item in textBoxes)
        {
            item.transform.GetChild(1).GetComponent<Text>().text = "Loading";
            item.transform.GetChild(2).GetComponent<Text>().text = "...";
            i++;
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
