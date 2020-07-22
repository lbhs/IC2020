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

    private GameObject UI;

    [SerializeField]
    private GameObject EndGamePrefab;

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

        UI = GameObject.Find("UI");
    }

    public void IncrementCountWrapper()
    {
        GetComponent<PhotonView>().RPC("IncrementCount", RpcTarget.All);
    }

    [PunRPC]
    private void IncrementCount()
    {
        if (GameSetupContrller.Instance.state == GameState.Player1Turn)
        {
            transform.GetChild(1).GetComponent<Text>().text = (++TotalTurnsDisplaying[0]).ToString();
            if (TotalTurnsDisplaying[0] >= 4)
            {
                // If each player has n turns, when Player 1 is about to press the die and start their (n + 1)th turn, load the win/lose/tie scene
                // Note that players cannot stall by pressing the End Turn button without rolling -- refer to the EndTurnButton() method of GameSetupContrller.cs for more information

                // EndGamePrefab has the GameEndInfo component, which records both players' scores and the identity of this player
                Instantiate(EndGamePrefab, Vector3.zero, Quaternion.identity);

                GameEndInfo.Player1Score = UI.transform.GetChild(6).GetComponent<TextController>().PreviousTotalScore;
                GameEndInfo.Player2Score = UI.transform.GetChild(7).GetComponent<TextController>().PreviousTotalScore;

                // We are trying to determine the current player
                // The GameObject that this component is attached to is a scene object, so its owner will be null
                // So, we scan the object hierarchy for the player GameObject that was instantiated in the GameSetupController
                GameObject MyPlayer = null;

                foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (GO.GetComponent<PhotonView>().IsMine)
                    {
                        MyPlayer = GO;
                    }
                }

                if (MyPlayer != null)
                    GameEndInfo.LocalPlayer = MyPlayer.GetComponent<PhotonView>().Owner;

                SceneManager.LoadScene(3);
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
        if (UI.transform.GetChild(1).GetComponent<Button>().interactable)
        {
            if (StartLobbyController.FinalUsername != null)
                transform.GetChild(0).GetComponent<Text>().text = StartLobbyController.FinalUsername;
            else
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

    public void DisplayTurnsP2()
    {
        transform.GetChild(1).GetComponent<Text>().text = (TotalTurnsDisplaying[1]).ToString();
        if (UI.transform.GetChild(1).GetComponent<Button>().interactable)
        {
            if (StartLobbyController.FinalUsername != null)
                transform.GetChild(0).GetComponent<Text>().text = StartLobbyController.FinalUsername;
            else
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
