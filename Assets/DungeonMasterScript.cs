using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0618

public class DungeonMasterScript : NetworkBehaviour
{
    public List<GameObject> Players = new List<GameObject>();
    //TO-DO if player disconnects, remove them fro the list
    [HideInInspector]
    public int PlayerTurn = 0; // to see who's turn it is (Players[0])

    public GameObject TurnScreen;

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

    }

    //--------Functions---------
    public void RollTheDice()
    {
        int roll = Random.Range(1, 6);

        if(roll == 1)
        {
            CmdSpawnAOxygen(new Vector3(0, 0, 0), 0);
        } 
    }
    
    public void EndTurn()
    {
        if(PlayerTurn == 0)
        {
            PlayerTurn = 1;
        }
        else
        {
            PlayerTurn = 0;
        }
        //To-do switch the Its not your turn screen
    }
    //---------Commands---------


    //Spawning Functions
    [Command]
    void CmdSpawnAOxygen(Vector3 position, int varient) //spawns a oxygen for whoever's turn it is
    {
        GameObject GO = Instantiate(OPrefab, position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(GO, Players[PlayerTurn].GetComponent<NetworkIdentity>().connectionToClient);
    }
}

//Notes
//NetworkServer.connections.Count
