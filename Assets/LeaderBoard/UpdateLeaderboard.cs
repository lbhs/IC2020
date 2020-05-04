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

public class Team
{
    public string TeamName;
    public string P1, P2, P3;
    public List<int> Score = new List<int>();
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
        //Debug.Log(J);
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

        for (int i = 0; i < 24; i++)
        {
            Team t = new Team();
            string TN = J["values"][i][2].Value;
            string P1 = J["values"][i][3].Value;
            string P2 = J["values"][i][4].Value;
            string P3 = J["values"][i][5].Value;
            t.TeamName = TN;
            t.P1 = P1;
            t.P2 = P2;
            t.P3 = P3;
            t.Score.Add(0);
            teams.Add(t);
            //Debug.Log(t.TeamName + " " + t.P1 + " " + t.P2 + " " + t.P3);
        }

        int forI = 0;
        foreach (var t in teams)
        {
            foreach (var p in players)
            {
                if (p.Name == t.P1 || p.Name == t.P2 || p.Name == t.P3)
                {
                    //Debug.Log(p.Name + " " + t.TeamName + " " + t.P1);
                    teams[forI].Score.Add(p.Score);
                    //Debug.Log(teams[forI].TeamName);
                }
            }
            teams[forI].Score = teams[forI].Score.OrderByDescending(i => i).ToList();
            //Debug.Log(t.Score[0]);
            forI++;
        }

        teams = teams.OrderByDescending(i => i.Score[0]).ToList();
        applyUpdates();
    }

    void applyUpdates()
    {

       
        //solitaire version code:

        int num = 0;
        foreach (var item in textBoxes)
        {
            item.transform.GetChild(0).GetComponent<Text>().text = num + 1 + ":";
            item.transform.GetChild(1).GetComponent<Text>().text = teams[num].TeamName;
            item.transform.GetChild(2).GetComponent<Text>().text = teams[num].Score[0].ToString();
            num++;
        }
        //Debug.Log(players[players.Count-1].Score);
    }

    void clear()
    {
        players.Clear();
        teams.Clear();
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
