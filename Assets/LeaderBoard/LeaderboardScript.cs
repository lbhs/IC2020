using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0618 // Type or member is obsolete
public class LeaderboardScript : MonoBehaviour
{
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdfRfapmI3she14Wkyye5eAhlzUIpyzc7iFDUUS1SVAWcdQzA/formResponse";
    //will return true if successful
    public bool Send(string email, int score)
    {
        /*
        if (!email.Contains("@lbusd.org"))
        {
            //not a valid name/score
            return false;
        }
        else
        {
            //code to format name
            string firstName;
            string lastName;
            string[] n = email.Split('@');
            string[] s = n[0].Split('.');
            firstName = s[0];
            firstName = char.ToUpper(firstName[0]) + firstName.Substring(1);
            lastName = s[1];
            lastName = char.ToUpper(lastName[0]) + lastName.Substring(1);
            string name = firstName + " " + lastName;
            StartCoroutine(Post(name, score.ToString()));
            return true;
        }*/

        StartCoroutine(Post(email, score.ToString()));
        return true;
    }

    //code from https://www.youtube.com/watch?v=z9b5aRfrz7M
    IEnumerator Post(string Name, string score)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1831576158", Name);
        form.AddField("entry.1054931094", score);
        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
