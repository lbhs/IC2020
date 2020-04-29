using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleSheetsToUnity;
using System.Linq;

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
        StartCoroutine(WorkAround());
        SpreadsheetManager.ReadPublicSpreadsheet(new GSTU_Search("163XTG4mOdzoAZv-HHcKGoRtDum-XvTOsajDYoiO97VA", "Leaderboard"), getScores);
    }

    void getScores(GstuSpreadSheet spreadsheetRef)
    {
        for (int i = 2; i < 999; i++)
        {
            var p = new Player();
            p.Name = spreadsheetRef["B" + i].value;
            p.Score = int.Parse(spreadsheetRef["C" + i].value);
            players.Add(p);
        }
    }

    void applyUpdates()
    {
        //players.Sort((s1, s2) => s1.Score.CompareTo(s2.Score));
        players = players.OrderByDescending(i => i.Score).ToList();
        int num = 0;
        foreach (var item in textBoxes)
        {
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

    IEnumerator WorkAround()
    {
        yield return new WaitForSecondsRealtime(5);
        applyUpdates();
    }
}
