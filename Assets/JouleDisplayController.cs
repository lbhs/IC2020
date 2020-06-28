using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    // The following two methods are only for use with the GSC during player transitions
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

    void Commit()
    {
        if (LastCopyJoulesDisplaying[0] < TotalJoulesDisplaying[0])
        {
            DisplayJoules(TotalJoulesDisplaying[0] - LastCopyJoulesDisplaying[0]);
            TotalJoulesDisplaying.CopyTo(LastCopyJoulesDisplaying, 0);
        }
        else if (LastCopyJoulesDisplaying[0] > TotalJoulesDisplaying[0])
        {
            Debug.Log("Decrementing joules for player 1");
            RemoveJoules(LastCopyJoulesDisplaying[0] - TotalJoulesDisplaying[0]);
            TotalJoulesDisplaying.CopyTo(LastCopyJoulesDisplaying, 0);
        }
        else if (LastCopyJoulesDisplaying[1] < TotalJoulesDisplaying[1])
        {
            DisplayJoules(TotalJoulesDisplaying[1] - LastCopyJoulesDisplaying[1]);
            TotalJoulesDisplaying.CopyTo(LastCopyJoulesDisplaying, 0);
        }
        else if (LastCopyJoulesDisplaying[1] > TotalJoulesDisplaying[1])
        {
            Debug.Log("Decrementing joules for player 2");
            RemoveJoules(LastCopyJoulesDisplaying[1] - TotalJoulesDisplaying[1]);
            TotalJoulesDisplaying.CopyTo(LastCopyJoulesDisplaying, 0);
        }
    }

    [PunRPC]
    public void IncrementJDC(int AmountToIncrement, int PVID, int ManuallySetBonus)
    {
        if (PhotonView.Find(PVID) != null)
        {
            GameSetupContrller GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();
            GameObject GO = PhotonView.Find(PVID).gameObject;
            if (GO.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[0])
            {
                TotalJoulesDisplaying[0] += AmountToIncrement;
                GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().BondScore += AmountToIncrement;
                if (ManuallySetBonus == 0)
                    GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().BonusScore += GSC.ReturnCompletionScore(GO.GetComponent<AtomController>().MoleculeID);
                else
                    GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().BonusScore += ManuallySetBonus;
            }
            else
            {
                TotalJoulesDisplaying[1] += AmountToIncrement;
                GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().BondScore += AmountToIncrement;
                if (ManuallySetBonus == 0)
                    GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().BonusScore += GSC.ReturnCompletionScore(GO.GetComponent<AtomController>().MoleculeID);
                else
                    GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().BonusScore += ManuallySetBonus;
            }
        }
        Commit();
    }
}
