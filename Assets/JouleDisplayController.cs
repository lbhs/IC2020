using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JouleDisplayController : MonoBehaviour
{
    // If TotalJoulesDisplaying and LastCopyJoulesDisplaying differ, then update the number of joules displaying
    public int[] TotalJoulesDisplaying;
    private int[] LastCopyJoulesDisplaying;

    // The joule prefab that is instantiated in the holder--value provided in the inspector
    public GameObject Joule;

    public static JouleDisplayController Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Index 0 is the number of joules belonging to Player 1, while Index 1 is the number of joules belonging to Player 2
        TotalJoulesDisplaying = new int[2] { 0, 0 };
        LastCopyJoulesDisplaying = new int[2] { 0, 0 };

        // Singleton design pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

    #region FOR GAMESETUPCONTRLLER USE ONLY
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
    #endregion

    [PunRPC]
    public void IncrementJDC(int AmountToIncrement, int PVID, int ManuallySetBonus)
    {
        // AmountToIncrement is added to the correct player's bond score
        // The GameObject identified by PVID is used to determine for which player points should be incremented/decremented
        // ManuallySetBonus adds bonus points for the correct player (manually)--for most cases, keep ManuallySetBonus at 0
        if (PhotonView.Find(PVID) != null)
        {
            GameObject GO = PhotonView.Find(PVID).gameObject;
            if (GO.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[0])
            {
                TotalJoulesDisplaying[0] += AmountToIncrement;
                GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().BondScore += AmountToIncrement;
                if (ManuallySetBonus == 0)
                    GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().BonusScore += MoleculeIDHandler.Instance.ReturnCompletionScore(GO.GetComponent<AtomController>().MoleculeID);
                else
                    GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().BonusScore += ManuallySetBonus;
            }
            else
            {
                TotalJoulesDisplaying[1] += AmountToIncrement;
                GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().BondScore += AmountToIncrement;
                if (ManuallySetBonus == 0)
                    GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().BonusScore += MoleculeIDHandler.Instance.ReturnCompletionScore(GO.GetComponent<AtomController>().MoleculeID);
                else
                    GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().BonusScore += ManuallySetBonus;
            }
        }

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
}
