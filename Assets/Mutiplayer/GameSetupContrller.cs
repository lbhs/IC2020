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
    private ComponentData CD;

    [HideInInspector]
    public Animator CamAnim;

    private GameObject RollPanelOptions;
    private List<List<GameObject>> MoleculeElements;

    public bool Unbonding;

    public List<GameObject> CurrentUnbonding;

    private void Start()
    {
        CD = GameObject.Find("ComponentReferences").GetComponent<ComponentData>();
        CreatePlayer();
        state = GameState.Start;
        PV = GetComponent<PhotonView>();
        CamAnim = Camera.main.GetComponent<Animator>();
        MoleculeElements = new List<List<GameObject>>();
        RollPanelOptions = GameObject.Find("UI").transform.GetChild(3).gameObject;
        Unbonding = false;
        CurrentUnbonding = new List<GameObject>();
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
        PV.RPC("JouleHolderTransition", RpcTarget.All);
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
    public void GenerateID(int PVID, int PeerPVID)
    {
        // Two elements with a MoleculeID of 0 collide
        GameObject GOToAdd = PhotonView.Find(PVID).gameObject;
        GameObject SecondGOToAdd = PhotonView.Find(PeerPVID).gameObject;

        GOToAdd.GetComponent<AtomController>().MoleculeID = MoleculeElements.Count + 1;
        SecondGOToAdd.GetComponent<AtomController>().MoleculeID = MoleculeElements.Count + 1;

        List<GameObject> NewMolecule = new List<GameObject>();
        NewMolecule.Add(GOToAdd);
        NewMolecule.Add(SecondGOToAdd);
        MoleculeElements.Add(NewMolecule);
    }

    [PunRPC]
    public void GenerateBatchID(int[] PVID)
    {
        List<GameObject> NewMolecule = new List<GameObject>();
        foreach (int IndPVID in PVID)
        {
            PhotonView.Find(IndPVID).gameObject.GetComponent<AtomController>().MoleculeID = MoleculeElements.Count + 1;
            NewMolecule.Add(PhotonView.Find(IndPVID).gameObject);
        }
        MoleculeElements.Add(NewMolecule);
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
        GameObject GOToAdd = PhotonView.Find(PVID).gameObject;
        if (GOToAdd.GetComponent<AtomController>().MoleculeID != 0)
            MoleculeElements[GOToAdd.GetComponent<AtomController>().MoleculeID - 1].Remove(GOToAdd);
        GOToAdd.GetComponent<AtomController>().MoleculeID = IDToMergeInto;
        if (IDToMergeInto != 0)
            MoleculeElements[IDToMergeInto - 1].Add(GOToAdd);
    }

    //[PunRPC]
    //public void RemoveElementFromMoleculeDB(int PVID)
    //{
    //    MoleculeElements[PhotonView.Find(PVID).GetComponent<AtomController>().MoleculeID].Remove(PhotonView.Find(PVID).gameObject);
    //}

    public int ReturnCompletionScore(int MoleculeID)
    {
        int MoleculeSize = 0;
        foreach (GameObject GO in MoleculeElements[MoleculeID - 1])
        {
            if (GO.GetComponent<AtomController>().BondingOpportunities != 0)
            {
                return 0;
            }
            MoleculeSize++;
        }

        if (MoleculeSize < 6)
        {
            return 10 * (MoleculeSize - 1);
        }
        else
        {
            return 60;
        }
    }

    public int NumElementsInMolecule(int MoleculeID) 
    {
        return MoleculeElements[MoleculeID - 1].Count;
    }

    [PunRPC]
    public void RemoveGivenElements(int MoleculeID)
    {
        foreach (GameObject GO in MoleculeElements[MoleculeID - 1])
        {
            GO.GetComponent<AtomController>().MoleculeID = 0;
        }
        MoleculeElements[MoleculeID - 1].Clear();
    }

    [PunRPC]
    public void RemoveSetOfElements(int[] GOToRemove)
    {
        for (int i = 0; i < GOToRemove.Length; i++)
        {
            GameObject ActualGOToRemove = PhotonView.Find(GOToRemove[i]).gameObject;
            MoleculeElements[ActualGOToRemove.GetComponent<AtomController>().MoleculeID - 1].Remove(ActualGOToRemove);
            ActualGOToRemove.GetComponent<AtomController>().MoleculeID = 0;
        }
    }

    public List<GameObject> GetElementsAtGivenPosition(int MoleculeID)
    {
        return MoleculeElements[MoleculeID - 1];
    }

    [PunRPC]
    public void CalExit()
    {
        UIAnim.SetTrigger("Exit");
        //PV.RPC("ExitAnimCam", RpcTarget.All);
        //Debug.Log("callExit");
    }

    [PunRPC]
    private void JouleHolderTransition()
    {
        if (state == GameState.Player1Turn)
        {
            CD.JDC.DisplayJoulesP1();
        }
        else
        {
            CD.JDC.DisplayJoulesP2();
        }
    }

    public void ChangeUnbondingState()
    {
        Unbonding = !Unbonding;
    }
}