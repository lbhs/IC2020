using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Start, Player1Turn, Player2Turn, End }

public class GameSetupContrller : MonoBehaviour
{ 
    public GameState state;
    private PhotonView PV;
    public GameObject TurnScreen;
    public GameObject OPrefab;
    public GameObject HPrefab;
    public GameObject CPrefab;
    public GameObject CLPrefab;
    public Animator UIAnim;
    [HideInInspector]
    public Animator CamAnim;
    public GameObject JouleHolder;
    public GameObject JoulePrefab;

    private GameObject P1Display;
    private GameObject P2Display;

    private GameObject RollPanelOptions;
    private List<List<GameObject>> MoleculeElements;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        state = GameState.Start;
        PV = GetComponent<PhotonView>();
        CamAnim = Camera.main.GetComponent<Animator>();
        MoleculeElements = new List<List<GameObject>>();
    }

    private void Awake()
    {
        RollPanelOptions = GameObject.Find("UI").transform.GetChild(3).gameObject;
        P1Display = GameObject.Find("UI").transform.GetChild(6).gameObject;
        P2Display = GameObject.Find("UI").transform.GetChild(7).gameObject;
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
        Vector3 InstantiationPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Roll == 1)
        {
            UIAnim.SetTrigger("H");
            //PV.RPC("AnimateRollMenu", RpcTarget.All, "H");
        }
        else if (Roll == 2)
        {
            UIAnim.SetTrigger("C");
            //PV.RPC("AnimateRollMenu", RpcTarget.All, "O");
        }
        else if (Roll == 3)
        {
            UIAnim.SetTrigger("O");
            //PV.RPC("AnimateRollMenu", RpcTarget.All, "C");
        }
        else if (Roll == 4)
        {
            UIAnim.SetTrigger("CL");
            // PV.RPC("AnimateRollMenu", RpcTarget.All, "CL");
        }
        else if (Roll == 5)
        {
            UIAnim.SetTrigger("DoubleOnly");
            // PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleOnly");
        }
        else if (Roll == 6)
        {
            UIAnim.SetTrigger("DoubleDown");
            // PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleDown");
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
        Debug.Log("Joule spawned!");
        GameObject GO;
        GO = Instantiate(JoulePrefab, JouleHolder.transform);
        GO.transform.localPosition = new Vector3(Random.Range(-35, 35), Random.Range(-35, 35), 0);
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
        }
        else if(state == GameState.Player2Turn)
        {
            Debug.Log("Player 2 turn ending");
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
            P1Display.SetActive(true);
            P2Display.SetActive(false);
        } 
        else
        {
            P2Display.SetActive(true);
            P1Display.SetActive(false);
        }
    }

    [PunRPC]
    public void GenerateID(int PVID, int PeerPVID)
    {
        // Two elements with a MoleculeID of 0 collide
        GameObject GOToAdd = PhotonView.Find(PVID).gameObject;
        GameObject SecondGOToAdd = PhotonView.Find(PVID).gameObject;

        List<GameObject> NewMolecule = new List<GameObject>();
        NewMolecule.Add(GOToAdd);
        NewMolecule.Add(SecondGOToAdd);
        MoleculeElements.Add(NewMolecule);
        GOToAdd.GetComponent<AtomController>().MoleculeID = MoleculeElements.IndexOf(NewMolecule) + 1;
        SecondGOToAdd.GetComponent<AtomController>().MoleculeID = MoleculeElements.IndexOf(NewMolecule) + 1;
    }

    [PunRPC]
    public void MergeMoleculeLists(int IDToMerge, int IDToMergeInto)
    {
        // Both IDToMerge and IDToMergeInto are not equal to 0
        for (int idx = 0; idx < MoleculeElements[IDToMerge - 1].Count; idx++)
        {
            GameObject GO = MoleculeElements[IDToMerge - 1][idx];
            GO.GetComponent<AtomController>().MoleculeID = IDToMergeInto;
            MoleculeElements[IDToMergeInto - 1].Add(GO);
            MoleculeElements[IDToMerge - 1][idx] = null;
        }
    }

    [PunRPC]
    public void TransferSingleElement(int PVID, int IDToMergeInto)
    {
        // The GameObject identified by PVID has a MoleculeID of 0
        GameObject GOToAdd = PhotonView.Find(PVID).gameObject;
        GOToAdd.GetComponent<AtomController>().MoleculeID = IDToMergeInto;
        MoleculeElements[IDToMergeInto - 1].Add(GOToAdd);
    }

    [PunRPC]
    public void ChangeScoreUniformly(int BondScore, int BonusScore)
    {
        if (state == GameState.Player1Turn)
        {
            P1Display.GetComponent<TextController>().BondScore = BondScore;
            P1Display.GetComponent<TextController>().BonusScore = BonusScore;
        }
        else
        {
            P2Display.GetComponent<TextController>().BondScore = BondScore;
            P2Display.GetComponent<TextController>().BonusScore = BonusScore;
        }
    }

    public void CalExit()
    {
        UIAnim.SetTrigger("Exit");
        //PV.RPC("ExitAnimCam", RpcTarget.All);
        //Debug.Log("callExit");
    }
}