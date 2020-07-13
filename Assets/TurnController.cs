using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class TurnController : MonoBehaviour
{
    public static TurnController Instance { get; private set; }

    public int[] TotalTurnsDisplaying;

    public static string PeerUserName;

    // Start is called before the first frame update
    private void Start()
    {
        // Index 0 is the number of joules belonging to Player 1, while Index 1 is the number of joules belonging to Player 2
        TotalTurnsDisplaying = new int[2] { 0, 0 };

        // Singleton design pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GetComponent<PhotonView>().RPC("AdvertiseUsername", RpcTarget.Others, StartLobbyController.FinalUsername);
    }

    public void IncrementCountWrapper()
    {
        GetComponent<PhotonView>().RPC("IncrementCount", RpcTarget.All);
    }

    [PunRPC]
    private void IncrementCount(PhotonMessageInfo info)
    {
        if (GameSetupContrller.Instance.state == GameState.Player1Turn)
        {
            transform.GetChild(1).GetComponent<Text>().text = (++TotalTurnsDisplaying[0]).ToString();
            if (TotalTurnsDisplaying[0] >= 4)
            {
                Debug.LogFormat("Player 1: {0} to Player 2: {1}", GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().PreviousTotalScore, GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().PreviousTotalScore);
                Debug.LogFormat("Sender: {0}", info.Sender);

                //if ((GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().PreviousTotalScore
                //    > GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().PreviousTotalScore)
                //    && info.photonView.IsMine)
                //{
                //    SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
                //}
                //else if (GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>().PreviousTotalScore
                //         < GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>().PreviousTotalScore)
                //{
                //    SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
                //}
                //else
                //{
                //    SceneManager.LoadScene("TieScene", LoadSceneMode.Single);
                //}
            }
        }
        else
        {
            transform.GetChild(1).GetComponent<Text>().text = (++TotalTurnsDisplaying[1]).ToString();
        }
    }

    public void DisplayTurnsP1()
    {
        transform.GetChild(1).GetComponent<Text>().text = (TotalTurnsDisplaying[0]).ToString();
        if (GameObject.Find("UI").transform.GetChild(1).GetComponent<Button>().interactable)
            transform.GetChild(0).GetComponent<Text>().text = "You";
        else
        {
            if (PeerUserName != null)
                transform.GetChild(0).GetComponent<Text>().text = PeerUserName;
            else
                transform.GetChild(0).GetComponent<Text>().text = "Peer";
        }
    }

    public void DisplayTurnsP2()
    {
        transform.GetChild(1).GetComponent<Text>().text = (TotalTurnsDisplaying[1]).ToString();
        if (GameObject.Find("UI").transform.GetChild(1).GetComponent<Button>().interactable)
        {
            transform.GetChild(0).GetComponent<Text>().text = "You";
        }
        else
        {
            if (PeerUserName != null)
                transform.GetChild(0).GetComponent<Text>().text = PeerUserName;
            else
                transform.GetChild(0).GetComponent<Text>().text = "Peer";
        }
    }

    [PunRPC]
    private void AdvertiseUsername(string name, PhotonMessageInfo info)
    {
        PeerUserName = name;
    }
}
