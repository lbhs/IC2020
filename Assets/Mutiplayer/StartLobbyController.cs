using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject StartButton; // joining a game button
    [SerializeField]
    private GameObject CancelButton; // the never mind button
    [SerializeField]
    private int RoomSize; // number of player in a game

    private InputField UsernameField;

    public static string FinalUsername;

    private void Start()
    {
        UsernameField = GameObject.Find("UI").transform.GetChild(1).transform.GetChild(0).GetComponent<InputField>();
        DontDestroyOnLoad(gameObject);
        gameObject.tag = "DontDestroyOnLoad";
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        StartButton.SetActive(false);
    }

    public void StartButtonFunction() //called when pressing the start button
    {
        StartButton.SetActive(false);
        CancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); // first tries to find a room to join 
        Debug.Log("Searching for rooms");
        FinalUsername = UsernameField.text;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Couldn't find room");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000); //Room name for the new room
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room " + randomRoomNumber, roomOps); //attempting to make room with those variables
        Debug.Log("Trying to create Room " + randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... trying to create room again");
        CreateRoom();
    }
    
    public void CancelJoining()
    {
        CancelButton.SetActive(false);
        StartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void UsernameEntered()
    {
        if (UsernameField.text.Length > 0)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }
}
