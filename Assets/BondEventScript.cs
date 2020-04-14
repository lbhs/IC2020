using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondEventScript : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject TwinColider;

    private GameSetupContrller GSC;
    private PhotonView PV;
    private TextController TC;
    private BondEnergyValues BEV;

    private void Start()
    {
        PV = transform.root.GetComponent<PhotonView>();
    }

    private void Awake()
    {
        GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();
        if (transform.root.GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[0])
        {
            TC = GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>();
        }
        else
        {
            TC = GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>();
        }
        BEV = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.GetComponent<PhotonView>().Owner == transform.root.GetComponent<PhotonView>().Owner 
            && collision.tag == "BondArea")
        {
            // Both colliders need to register a collision for bonding to occur
            isTriggered = true;
            if (TwinColider.GetComponent<BondEventScript>().isTriggered == true)
            {
                transform.root.GetComponent<AtomController>().DecrementOnce();
                transform.root.GetComponent<AtomController>().BondingFunction(collision);

                if (!GSC.InSubList(transform.root.gameObject, collision.gameObject.transform.root
                                                              .GetComponent<AtomController>().MoleculeElementsIdx))
                {
                    int MergeFrom = transform.root.GetComponent<AtomController>().MoleculeElementsIdx;
                    int MergeInto = collision.gameObject.transform.root.GetComponent<AtomController>().MoleculeElementsIdx;

                    GSC.gameObject.GetComponent<PhotonView>().RPC("MergeMoleculeLists", RpcTarget.All, MergeFrom, MergeInto);
                }

                TC.BonusScore = GSC.TotalBonusPoints(transform.root.gameObject);
                TC.BondScore += BEV.bondEnergyArray[transform.root.GetComponent<AtomController>().EnergyMatrixPosition,
                                                       collision.transform.root.GetComponent<AtomController>().EnergyMatrixPosition];

            }
        }
    }   

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTriggered = false;
    }
}
