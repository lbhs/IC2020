﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum GameState { Start, Player1Turn, Player2Turn, End} 

public class GameSetupContrller : MonoBehaviour
{
    public GameState state;
    private PhotonView PV;
    public GameObject TurnScreen;
    public Button DiceButton;
    public GameObject OPrefab;
    public GameObject NPrefab;
    public GameObject HPrefab;
    public GameObject NAPrefab;
    public GameObject CPrefab;
    public GameObject CLPrefab;
    public Animator UIAnim;
    [HideInInspector]
    public Animator CamAnim;
    public GameObject JouleHolder;
    public GameObject JoulePrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendToLeaderboard("v",23));
        CreatePlayer();
        state = GameState.Start;
        PV = GetComponent<PhotonView>();
        CamAnim = Camera.main.GetComponent<Animator>();
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

    public void RollDice(int Roll)
    {
 
        if (Roll == 1)
        {
            UIAnim.ResetTrigger("Exit");
            UIAnim.SetTrigger("H");
            //PV.RPC("AnimateRollMenu", RpcTarget.All, "H");
        }
        else if (Roll == 2)
        {
            UIAnim.ResetTrigger("Exit");
            UIAnim.SetTrigger("C");
            //PV.RPC("AnimateRollMenu", RpcTarget.All, "O");
        }
        else if (Roll == 3)
        {
            UIAnim.ResetTrigger("Exit");
            UIAnim.SetTrigger("O");
            //PV.RPC("AnimateRollMenu", RpcTarget.All, "C");
        }
        else if (Roll == 4)
        {
            UIAnim.ResetTrigger("Exit");
            UIAnim.SetTrigger("CL");
            // PV.RPC("AnimateRollMenu", RpcTarget.All, "CL");
        }
        else if (Roll == 5)
        {
            UIAnim.ResetTrigger("Exit");
            UIAnim.SetTrigger("DoubleOnly");
            // PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleOnly");
        }
        else if (Roll == 6)
        {
            UIAnim.ResetTrigger("Exit");
            UIAnim.SetTrigger("DoubleDown");
            // PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleDown");
        }
        //NetowrkSpawn(OPrefab, Vector3.zero);
       // SpawnJoule();
       // SpawnJoule();
    }

    IEnumerator SendToLeaderboard(string name, int score)
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://dreamlo.com/lb/" + GetComponent<SecretCode>().code + "/add/" + name + "/" + score);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    public void NetowrkSpawn(GameObject Prefab, Vector3 pos)
    {
        GameObject GO;
        if (state == GameState.Player1Turn)
        {
            GO = PhotonNetwork.Instantiate(Prefab.name, pos, Quaternion.identity);
            GO.GetComponent<PhotonView>().RequestOwnership();
        }
        else if (state == GameState.Player2Turn)
        {
            GO = PhotonNetwork.Instantiate(Prefab.name, pos, Quaternion.identity);
            GO.GetComponent<PhotonView>().RequestOwnership();
        }
    }

    public void SpawnJoule()
    {
        GameObject GO;
        GO = Instantiate(JoulePrefab, JouleHolder.transform);
        GO.transform.localPosition = new Vector3(Random.Range(-35, 35), Random.Range(-35, 35), 0);
    }

    public void EndTurnButton()
    {
        if(state == GameState.Player1Turn)
        {
            PV.RPC("ChangeState", RpcTarget.All, GameState.Player2Turn);
            PV.RPC("StartTurn", PhotonNetwork.PlayerList[1]);
            PV.RPC("EndTurn", PhotonNetwork.PlayerList[0]);
            PV.RPC("AnimateCam", RpcTarget.All, false);
        }
        else if(state == GameState.Player2Turn)
        {
            PV.RPC("ChangeState", RpcTarget.All, GameState.Player1Turn);
            PV.RPC("StartTurn", PhotonNetwork.PlayerList[0]);
            PV.RPC("EndTurn", PhotonNetwork.PlayerList[1]);
            PV.RPC("AnimateCam", RpcTarget.All, true);
        }
        if (!UIAnim.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
        {
            UIAnim.SetTrigger("Exit");
        }
    }

    public void CalExit()
    {
        UIAnim.SetTrigger("Exit");
        //PV.RPC("ExitAnimCam", RpcTarget.All);
        //Debug.Log("callExit");
    }

    /*[PunRPC]
    public void ExitAnimCam()
    {
        UIAnim.SetTrigger("Exit");
        Debug.Log("rpcexit");
    }*/

    [PunRPC]
    public void AnimateCam(bool b)
    {
        CamAnim.SetBool("Player1Turn", b);
    }

   /* [PunRPC]
    public void AnimateRollMenu(string s)
    {
        UIAnim.SetTrigger(s);
    }*/

    [PunRPC]
    public void EndTurn()
    {
        //Debug.Log("1");
        TurnScreen.SetActive(true);
        DiceButton.interactable = false;
    }

    [PunRPC]
    public void StartTurn()
    {
        //Debug.Log("2");
        TurnScreen.SetActive(false);
        DiceButton.interactable = true;
    }
    
    [PunRPC]
    public void ChangeState(GameState s)
    {
        state = s;
    }
}