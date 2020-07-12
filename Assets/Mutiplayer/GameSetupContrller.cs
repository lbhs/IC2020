using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Start, Player1Turn, Player2Turn, End }

public class GameSetupContrller : MonoBehaviour
{
    #region Public Member Variables
    public GameState state;
    public GameObject TurnScreen;
    public Animator UIAnim;
    public bool Unbonding;
    public PhotonView PV;
    public static GameSetupContrller Instance { get; private set; }
    public static string PeerUserName;
    #endregion

    #region Private Member Variables
    [HideInInspector]
    public Animator CamAnim;

    private GameObject RollPanelOptions;
    #endregion

    private void Start()
    {
        state = GameState.Start;
        PV = GetComponent<PhotonView>();
        // Camera is moved to view Player 1's and Player 2's actions
        CamAnim = Camera.main.GetComponent<Animator>();
        RollPanelOptions = GameObject.Find("UI").transform.GetChild(3).gameObject;
        Unbonding = false;
        CreatePlayer(); 

        // Singleton design pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
            PV.RPC("TurnsStateTransition", RpcTarget.All);
            PV.RPC("ChangeScreenDisplaying", RpcTarget.All, GameState.Player1Turn);
        }
    }

    public void RollDice(int Roll)
    {
        if (Roll == 1)
        {
            PV.RPC("AnimateRollMenu", RpcTarget.All, "H");
        }
        else if (Roll == 2)
        {
            PV.RPC("AnimateRollMenu", RpcTarget.All, "O");
        }
        else if (Roll == 3)
        {
            PV.RPC("AnimateRollMenu", RpcTarget.All, "C");
        }
        else if (Roll == 4)
        {
            PV.RPC("AnimateRollMenu", RpcTarget.All, "CL");
        }
        else if (Roll == 5)
        {
            PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleOnly");
        }
        else if (Roll == 6)
        {
            PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleDown");
        }
    }

    public void NetowrkSpawn(GameObject Prefab, Vector3 pos)
    {
        GameObject GO;
        GO = PhotonNetwork.Instantiate(Prefab.name, pos, Quaternion.identity);
        GO.GetComponent<PhotonView>().RequestOwnership();
    }

    public void EndTurnButton()
    { 
        if(state == GameState.Player1Turn)
        {
            Debug.Log("Player 1 turn ending");
            PV.RPC("ChangeState", RpcTarget.All, GameState.Player2Turn);
            PV.RPC("StartTurn", PhotonNetwork.PlayerList[1]);
            PV.RPC("EndTurn", PhotonNetwork.PlayerList[0]);
            PV.RPC("AnimateCam", RpcTarget.All, false);
            PV.RPC("ChangeScreenDisplaying", RpcTarget.All, GameState.Player2Turn);
        }
        else if(state == GameState.Player2Turn)
        {
            Debug.Log("Player 2 turn ending");
            PV.RPC("ChangeState", RpcTarget.All, GameState.Player1Turn);
            PV.RPC("StartTurn", PhotonNetwork.PlayerList[0]);
            PV.RPC("EndTurn", PhotonNetwork.PlayerList[1]);
            PV.RPC("AnimateCam", RpcTarget.All, true);
            PV.RPC("ChangeScreenDisplaying", RpcTarget.All, GameState.Player1Turn);
        }
        PV.RPC("StateTransition", RpcTarget.All);
        PV.RPC("TurnsStateTransition", RpcTarget.All);
    }

    [PunRPC]
    public void AnimateCam(bool b)
    {
        CamAnim.SetBool("Player1Turn", b);
    }

    [PunRPC]
    public void AnimateRollMenu(string s)
    {
        UIAnim.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        UIAnim.SetTrigger(s);
    }

    [PunRPC]
    public void EndTurn()
    {
        //Debug.Log("1");
        TurnScreen.SetActive(true);
        // DieScript.rolling = 1;
        GameObject.Find("UI").transform.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
        GameObject.Find("UI").transform.GetChild(8).gameObject.GetComponent<Button>().interactable = false;
        // Debug.Log("No longer rolling: " + DieScript.rolling);
    }

    [PunRPC]
    public void StartTurn()
    {
        //Debug.Log("2");
        TurnScreen.SetActive(false);
        // DieScript.rolling = 0;
        // PV.RPC("ChangeScreenDisplaying", RpcTarget.All, state);
        GameObject.Find("UI").transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
        GameObject.Find("UI").transform.GetChild(8).gameObject.GetComponent<Button>().interactable = true;
        // Debug.Log("Rolling: " + DieScript.rolling);
    }
    
    [PunRPC]
    public void ChangeState(GameState s)
    {
        state = s;
    }

    [PunRPC]
    private void ChangeScreenDisplaying(GameState s)
    {
        if (s == GameState.Player1Turn)
        {
            GameObject.Find("UI").transform.GetChild(6).gameObject.SetActive(true);
            GameObject.Find("UI").transform.GetChild(7).gameObject.SetActive(false);
        } 
        else
        {
            GameObject.Find("UI").transform.GetChild(6).gameObject.SetActive(false);
            GameObject.Find("UI").transform.GetChild(7).gameObject.SetActive(true);
        }
    }

    [PunRPC]
    public void CalExit()
    {
        UIAnim.SetTrigger("Exit");
    }

    [PunRPC]
    public void ChangeAnimBoolExiting(bool state)
    {
        UIAnim.SetBool("Exiting", state);
    }

    [PunRPC]
    private void StateTransition()
    {
        if (state == GameState.Player1Turn)
            JouleDisplayController.Instance.DisplayJoulesP1();
        else
            JouleDisplayController.Instance.DisplayJoulesP2();
    }

    [PunRPC]
    private void TurnsStateTransition()
    {
        if (state == GameState.Player1Turn)
            TurnController.Instance.DisplayTurnsP1();
        else
            TurnController.Instance.DisplayTurnsP2();
    }

    public void ChangeUnbondingState()
    {
        Unbonding = !Unbonding;
    }
}