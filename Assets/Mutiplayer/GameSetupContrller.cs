using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum GameState { Start, Player1Turn, Player2Turn, End} 

public class GameSetupContrller : MonoBehaviour
{
    public GameState state;
    private PhotonView PV;
    public GameObject TurnScreen;
    public GameObject OPrefab;
    public GameObject NPrefab;
    public GameObject HPrefab;
    public GameObject NAPrefab;
    public GameObject CPrefab;
    public GameObject CLPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        state = GameState.Start;
        PV = GetComponent<PhotonView>();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerPreafab"), Vector3.zero, Quaternion.identity);
    }

     void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if(state == GameState.Start)
        {
            PV.RPC("ChangeState", RpcTarget.All, GameState.Player1Turn);
            PV.RPC("StartTurn", PhotonNetwork.PlayerList[0]);
            PV.RPC("EndTurn", PhotonNetwork.PlayerList[1]);
        }
    }

    public void RollDice()
    {
        PhotonNetwork.Instantiate(this.OPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public void EndTurnButton()
    {

    }

    [PunRPC]
    public void EndTurn()
    {
        Debug.Log("1");
        TurnScreen.SetActive(true);
    }

    [PunRPC]
    public void StartTurn()
    {
        Debug.Log("2");
        TurnScreen.SetActive(false);
    }
    
    [PunRPC]
    public void ChangeState(GameState s)
    {
        state = s;
    }
}