using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JouleDisplayController : MonoBehaviour
{
    public int[] TotalJoulesDisplaying;
    private int[] LastCopyJoulesDisplaying;
    public GameObject Joule;

    // Start is called before the first frame update
    void Start()
    {
        TotalJoulesDisplaying = new int[2] { 0, 0 };
        LastCopyJoulesDisplaying = new int[2] { 0, 0 };
    }

    private void DisplayJoules(int JouleCount)
    {
        for (int i = 0; i < JouleCount; i++)
        {
            GameObject GO;
            GO = Instantiate(Joule, gameObject.transform);
            GO.transform.localPosition = new Vector3(Random.Range(-35, 35), Random.Range(-35, 35), 0);
        }
    }

    private void RemoveJoules(int JoulesToRemove)
    {
        for (int i = 0; i < JoulesToRemove; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    // The following methods are only for use with the GSC during player transitions
    public void DisplayJoulesP1()
    {
        RemoveJoules(TotalJoulesDisplaying[1]);
        DisplayJoules(TotalJoulesDisplaying[0]);
    }

    public void DisplayJoulesP2()
    {
        RemoveJoules(TotalJoulesDisplaying[0]);
        DisplayJoules(TotalJoulesDisplaying[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (LastCopyJoulesDisplaying[0] != TotalJoulesDisplaying[0])
        {
            DisplayJoules(TotalJoulesDisplaying[0] - LastCopyJoulesDisplaying[0]);
            TotalJoulesDisplaying.CopyTo(LastCopyJoulesDisplaying, 0);
        }
        else if (LastCopyJoulesDisplaying[1] != TotalJoulesDisplaying[1])
        {
            DisplayJoules(TotalJoulesDisplaying[1] - LastCopyJoulesDisplaying[1]);
            TotalJoulesDisplaying.CopyTo(LastCopyJoulesDisplaying, 0);
        }
    }
}
