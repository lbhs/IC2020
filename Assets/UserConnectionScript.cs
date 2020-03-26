using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#pragma warning disable CS0618

public class UserConnectionScript : NetworkBehaviour
{
    public string Username = "no name yet";
    public GameObject TurnScreen;
    public GameObject TestBoxPrefab;
    private Button EndtTurnButton;


    // Start is called before the first frame update
    void Start()
    {
       // DM = GameObject.Find("DungeonMaster").GetComponent<DungeonMasterScript>();
        if (isLocalPlayer == false)
        {
            return;
        }
        EndtTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        EndtTurnButton.onClick.AddListener(delegate { CmdEndTurn(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject.Find("DungeonMaster").GetComponent<DungeonMasterScript>().RollTheDice();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            CmdUpdateUsername("Joe");
        }
    }

    //executes on server

    [Command]
    public void CmdEndTurn()
    {
        GameObject.Find("DungeonMaster").GetComponent<DungeonMasterScript>().EndTurn();
        Debug.Log("presssed end turn");
    }

    [Command]
    void CmdSpawnABox()
    {
        GameObject Box = Instantiate(TestBoxPrefab);
        NetworkServer.SpawnWithClientAuthority(Box, connectionToClient);
    }

   
    [Command]
    void CmdUpdateUsername(string n)
    {
        Username = n;
        RpcUpdateUsername(n);
    }

    //executes on everyone's computer
    [ClientRpc]
    void RpcUpdateUsername(string n)
    {
        Username = n;
        
    }
}
