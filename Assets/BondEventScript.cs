using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondEventScript : MonoBehaviour
{
    public GameObject TwinColider;

    private GameSetupContrller GSC;
    private PhotonView PV;

    public bool Bonded;

    private void Start()
    {
        PV = transform.root.GetComponent<PhotonView>();
        Bonded = false;
    }

    private void Awake()
    {
        GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.GetComponent<PhotonView>().Owner == transform.root.GetComponent<PhotonView>().Owner 
            && ((tag == "Peak" && collision.tag == "Valley") || (tag == "Valley" && collision.tag == "Peak")))
        {
            // Problematic: what happens if isTriggered hasn't registered yet?
            if (TwinColider.GetComponent<LiteController>().isTriggered == true)
            {
                collision.GetComponent<LiteController>().TwinColider.GetComponent<BondEventScript>().Bonded = true;
                Bonded = true;
                transform.root.GetComponent<AtomController>().BondingFunction(collision);

                if (!GSC.InSubList(transform.root.gameObject, collision.gameObject.transform.root
                                                              .GetComponent<AtomController>().MoleculeElementsIdx))
                {
                    int MergeFrom = transform.root.GetComponent<AtomController>().MoleculeElementsIdx;
                    int MergeInto = collision.gameObject.transform.root.GetComponent<AtomController>().MoleculeElementsIdx;

                    GSC.gameObject.GetComponent<PhotonView>().RPC("MergeMoleculeLists", RpcTarget.All, MergeFrom, MergeInto);
                }
            }
        }
    }   
}
