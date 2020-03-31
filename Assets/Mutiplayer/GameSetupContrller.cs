using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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
        NetowrkSpawn(OPrefab);
        SpawnJoule();
        SpawnJoule();
        if(Roll == 5)
        {
            PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleOnly");
        }
        if(Roll == 6)
        {
            PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleDown");
        }
    }

    public void NetowrkSpawn(GameObject Prefab)
    {
        GameObject GO;
        if (state == GameState.Player1Turn)
        {
            GO = PhotonNetwork.Instantiate(Prefab.name, new Vector3(0,5,0), Quaternion.identity);
            GO.GetComponent<PhotonView>().RequestOwnership();
        }
        else if (state == GameState.Player2Turn)
        {
            GO = PhotonNetwork.Instantiate(Prefab.name, new Vector3(0, -5, 0), Quaternion.identity);
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
    }

    [PunRPC]
    public void AnimateCam(bool b)
    {
        CamAnim.SetBool("Player1Turn", b);
    }

    [PunRPC]
    public void AnimateRollMenu(string s)
    {
        UIAnim.SetTrigger(s);
    }

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