using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
 
#pragma warning disable CS0618

public enum GameState {Start, Player1Turn, Player2Turn,End}

public class DungeonMasterScript : NetworkBehaviour 
{
    public List<GameObject> Players = new List<GameObject>();
    //TO-DO if player disconnects, remove them fro the list
    public GameState state;

    public bool allPlayerHere = false;

    [Header("Objects to spawn")]
    public GameObject HPrefab;
    public GameObject NAPrefab;
    public GameObject CPrefab;
    public GameObject OPrefab;
    public GameObject CLPrefab;
    public GameObject NPrefab;

    private NetworkConnection Player1;
    private NetworkConnection Player2;

    void Start()
    {

    }

    void Update()
    {
        Debug.Log(NetworkServer.connections);
        if (!allPlayerHere)
        {
            return;
        }
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
        if(0 == 0)
        {
           /// PlayerTurn = 1;
            TargetToggleItsNotYourTurnScreen(Player1,true);
        }
        else
        {
           // PlayerTurn = 0;
        }
        //To-do switch the Its not your turn screen
    }

    [TargetRpc]
    void TargetToggleItsNotYourTurnScreen(NetworkConnection Target, bool turnOn)
    {
        if (turnOn == true)
        {
            //TurnScreen.SetActive(true);
            Debug.Log("test2");
        }
        else
        {
            //TurnScreen.SetActive(true);
            Debug.Log("test");
        }
    }

    //Spawning Functions
    [Command]
    void CmdSpawnPrefab(GameObject Prefab,Vector3 position, int varient) //spawns a oxygen for whoever's turn it is
    {
        GameObject GO = Instantiate(Prefab, position, Quaternion.identity);
        //NetworkServer.SpawnWithClientAuthority(GO, Players[PlayerTurn].GetComponent<NetworkIdentity>().connectionToClient);
    }
   
}

