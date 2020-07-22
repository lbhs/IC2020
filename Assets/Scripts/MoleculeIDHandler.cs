using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MoleculeIDHandler : MonoBehaviour
{
    private List<List<GameObject>> MoleculeElements;
    public static MoleculeIDHandler Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        MoleculeElements = new List<List<GameObject>>();

        // Singleton design pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #region Creating a new ID
    [PunRPC]
    public void GenerateID(int PVID, int PeerPVID)
    {
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
    #endregion

    #region Transferring existing IDs

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

    #endregion

    #region Getter Methods (no updates performed on MoleculeElements)

    public int ReturnCompletionScore(int MoleculeID)
    {
        int MoleculeSize = 0;
        foreach (GameObject GO in MoleculeElements[MoleculeID - 1])
        {
            Debug.LogFormat("{0} has {1} bonding opportunities", GO.name, GO.GetComponent<AtomController>().BondingOpportunities);
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

    public List<GameObject> GetElementsAtGivenPosition(int MoleculeID)
    {
        return MoleculeElements[MoleculeID - 1];
    }

    public int NumElementsInMolecule(int MoleculeID)
    {
        return MoleculeElements[MoleculeID - 1].Count;
    }

    #endregion

    #region Unassigning MoleculeIDs
    // Formerly RemoveGivenElements
    [PunRPC]
    public void RemoveElementListByID(int MoleculeID)
    {
        foreach (GameObject GO in MoleculeElements[MoleculeID - 1])
        {
            GO.GetComponent<AtomController>().MoleculeID = 0;
        }
        MoleculeElements[MoleculeID - 1].Clear();
    }

    // Formerly RemoveSetOfElements

    [PunRPC]
    public void RemoveArrayOfElements(int[] GOToRemove)
    {
        for (int i = 0; i < GOToRemove.Length; i++)
        {
            GameObject ActualGOToRemove = PhotonView.Find(GOToRemove[i]).gameObject;
            MoleculeElements[ActualGOToRemove.GetComponent<AtomController>().MoleculeID - 1].Remove(ActualGOToRemove);
            ActualGOToRemove.GetComponent<AtomController>().MoleculeID = 0;
        }
    }
    #endregion

    public void ViewContentsOfID()
    {
        int MoleculeID = int.Parse(GameObject.Find("UI").transform.GetChild(12).transform.GetChild(2).GetComponent<Text>().text);
        string elements = "";
        foreach (GameObject GO in MoleculeElements[MoleculeID - 1])
        {
            elements += GO.name;
        }
        Debug.Log(elements);
    }
}
