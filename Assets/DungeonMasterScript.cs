using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
 
#pragma warning disable CS0618

public enum GameState {Start, Player1Turn, Player2Turn,End}

public class DungeonMasterScript : NetworkBehaviour 
{
    List<GameObject> Players = new List<GameObject>();
    //TO-DO handle disconnects 

    public GameState state;

    public GameObject TurnScreen;
    public GameObject WaitingForPlayersScreen;

    [Header("Objects to spawn")]
    public GameObject HPrefab;
    public GameObject NAPrefab;
    public GameObject CPrefab;
    public GameObject OPrefab;
    public GameObject CLPrefab;
    public GameObject NPrefab;

    void Start()
    {

    }

    void Update()
    {
        if (isServer == false)
        {
            return;
        }

        foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!Players.Contains(p))
            {
                Players.Add(p);
            }
        }

        if (state == GameState.Start)
        {
            if (Players.Count < 2)
            {
                return;
            }
            else
            {
                RpcWaitingScreen();
            }
        }
        else if(state == GameState.Player1Turn)
        {
            TargetToggleItsNotYourTurnScreen(Players[1].GetComponent<NetworkIdentity>().clientAuthorityOwner, true);
        }
    }

    [ClientRpc]
    void RpcWaitingScreen()
    {
        if (GameObject.Find("Waiting for Players") != false)
        {
            GameObject.Find("Waiting for Players").SetActive(false);
        }
        RpcSwitchState(GameState.Player1Turn);
    }

    [ClientRpc]
    void RpcSwitchState(GameState s)
    {
        state = s;
    }

    //--------Functions---------
    public void RollTheDice()
    {
        int roll = Random.Range(1, 6);

        if(roll == 1)
        {
            CmdSpawnPrefab(OPrefab, new Vector3(0, 0, 0), 0);
        } 
    }
    
    public void EndTurn()
    {
        Debug.Log("recived");
     
       Debug.Log("ended turn");
       if(state == GameState.Player1Turn)
        {
            TargetToggleItsNotYourTurnScreen(Players[0].GetComponent<NetworkIdentity>().clientAuthorityOwner, true);
            TargetToggleItsNotYourTurnScreen(Players[1].GetComponent<NetworkIdentity>().clientAuthorityOwner, false);
            state = GameState.Player2Turn;
        }
        else if(state == GameState.Player2Turn)
        {
            TargetToggleItsNotYourTurnScreen(Players[0].GetComponent<NetworkIdentity>().clientAuthorityOwner, false);
            TargetToggleItsNotYourTurnScreen(Players[1].GetComponent<NetworkIdentity>().clientAuthorityOwner, true);
            state = GameState.Player1Turn;
            
        }
    }

    [TargetRpc]
    void TargetToggleItsNotYourTurnScreen(NetworkConnection Target, bool turnOn)
    {
        if (turnOn == true)
        {
            //TurnScreen.SetActive(true);
            //Debug.Log("test2");
            //Target.clientOwnedObjects[0].
            TurnScreen.SetActive(true);
        }
        else
        {
            TurnScreen.SetActive(false);
            //Debug.Log("test");
        }
    }

    //Spawning Functions
    [Command]
    void CmdSpawnPrefab(GameObject Prefab,Vector3 position, int varient) //spawns a oxygen for whoever's turn it is
    {
        int num;
        if (state == GameState.Player1Turn)
            num = 0;
        else if (state == GameState.Player2Turn)
            num = 1;
        else
            return;
        Debug.Log(num);
        GameObject GO = Instantiate(Prefab, position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(GO, Players[num].GetComponent<NetworkIdentity>().clientAuthorityOwner);
    }
   
}

