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

public class UpdateLeaderboard : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public GameObject[] textBoxes;

    private void OnEnable()
    {
        clear();
        StartCoroutine(GetString());
        //"163XTG4mOdzoAZv-HHcKGoRtDum-XvTOsajDYoiO97VA", "Leaderboard")
    }

    IEnumerator GetString()
    {
        string url = "https://sheets.googleapis.com/v4/spreadsheets/163XTG4mOdzoAZv-HHcKGoRtDum-XvTOsajDYoiO97VA/values/Leaderboard!" + "B2" + ":" + "C1001" + "?key=" + SecretKey.GSkey;
        WWW www = new WWW(url);
        yield return www;
        string RecivedJSON;
        RecivedJSON = www.text;
        var J = JSON.Parse(RecivedJSON);
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
        applyUpdates();

        /*string url1 = "https://sheets.googleapis.com/v4/spreadsheets/163XTG4mOdzoAZv-HHcKGoRtDum-XvTOsajDYoiO97VA/values/Leaderboard!B" + row + ":B" + row + "?key=" + SecretKey.GSkey;
        WWW www = new WWW(url1);
        yield return www;
        RecivedJSON = www.text;
        var J = JSON.Parse(RecivedJSON);
        string name = J["values"][0][0].Value;

        string url2 = "https://sheets.googleapis.com/v4/spreadsheets/163XTG4mOdzoAZv-HHcKGoRtDum-XvTOsajDYoiO97VA/values/Leaderboard!C" + row + ":C" + row + "?key=" + SecretKey.GSkey;
        WWW www2 = new WWW(url2);
        yield return www2;
        RecivedJSON = www2.text;
        var J2 = JSON.Parse(RecivedJSON);
        string score = J2["values"][0][0].Value;

        if (name == null || score == null)
        {
            var p = new Player();
            p.Name = name;
            p.Score = int.Parse(score);
            players.Add(p);
            Debug.Log(p.Name + " " + p.Score);
        }
        else
        {
            print("found empty cell");
        }*/
    }

    void applyUpdates()
    {
        //players.Sort((s1, s2) => s1.Score.CompareTo(s2.Score));
        players = players.OrderByDescending(i => i.Score).ToList();
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
