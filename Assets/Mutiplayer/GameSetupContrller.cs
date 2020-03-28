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

    void RollDice()
    {
        //PhotonNetwork.
    }

    [PunRPC]
    public void EndTurn()
    {
        Debug.Log("1");
    }

    [PunRPC]
    public void StartTurn()
    {
        Debug.Log("2");
    }
    
    [PunRPC]
    public void ChangeState(GameState s)
    {
        state = s;
    }
}