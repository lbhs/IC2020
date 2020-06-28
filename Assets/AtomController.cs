using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomController : MonoBehaviourPunCallbacks
{
    private PhotonView PV;

    public bool isBonded = false;

    public int MoleculeID;

    public int BondingOpportunities;

    public int EnergyMatrixPosition;

    private BondEnergyValues BEV;

    public bool CollisionEventRegistered;

    // PhotonView ID -- used to retrieve the GameObject during RPC calls
    public int PVID;

    public bool Monovalent;

    private TextController ScoringSystem;

    GameObject peer;

    private ComponentData CD;

    private int BondEnergy;

    void Start()
    {
        CD = GameObject.Find("ComponentReferences").GetComponent<ComponentData>();

        BEV = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>();

        CollisionEventRegistered = false;

        PV = GetComponent<PhotonView>();
        PVID = PV.ViewID;

        BondEnergy = 0;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Peak" || collider.tag == "PeakDB")
        {
            peer = collider.transform.root.gameObject;

            if (UnbondingScript2.DontBondAgain > 0)
            {
                Debug.Log("Cannot bond yet");
                return;
            }

            // If the bonding elements are not mine, the joint mechanism is implemented differently to prevent crashes
            if (PV.IsMine)
            {
                CollisionEventRegistered = true;
                if (peer.GetComponent<AtomController>().CollisionEventRegistered)
                {
                    // RPCs are used for the ID methods because these changes should affect both players' ID databases
                    if (MoleculeID == 0 && peer.GetComponent<AtomController>().MoleculeID == 0)
                    {
                        CD.GameSetupPhotonView.RPC("GenerateID", RpcTarget.All, PVID, peer.GetComponent<AtomController>().PVID);
                        Debug.Log("GenerateID called");
                    }

                    else if (MoleculeID == 0 && peer.GetComponent<AtomController>().MoleculeID > 0)
                    {
                        CD.GameSetupPhotonView.RPC("TransferSingleElement", RpcTarget.All, PVID, peer.GetComponent<AtomController>().MoleculeID);
                        Debug.Log("TransferSingleElement called");
                    }

                    else if (MoleculeID > 0 && peer.GetComponent<AtomController>().MoleculeID == 0)
                    {
                        CD.GameSetupPhotonView.RPC("TransferSingleElement", RpcTarget.All, peer.GetComponent<AtomController>().PVID, MoleculeID);
                        Debug.Log("TransferSingleElement called");
                    }

                    else
                    {
                        CD.GameSetupPhotonView.RPC("MergeMoleculeLists", RpcTarget.All, MoleculeID, peer.GetComponent<AtomController>().MoleculeID);
                        Debug.Log("MergeMoleculeLists called");
                    }

                    // Optimized bonding
                    if (Monovalent)
                    {
                        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                        joint.connectedBody = peer.GetComponent<Rigidbody2D>();
                        joint.enableCollision = false;
                    }

                    // The other element solely is monovalent
                    else if (peer.GetComponent<AtomController>().Monovalent)
                    {
                        FixedJoint2D joint = peer.AddComponent<FixedJoint2D>();
                        joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                        joint.autoConfigureConnectedAnchor = false;
                        joint.enableCollision = false;
                        joint.dampingRatio = 1f;
                    }

                    else
                    {
                        FixedJoint2D joint = peer.AddComponent<FixedJoint2D>();
                        joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                        joint.autoConfigureConnectedAnchor = false;
                        joint.enableCollision = false;
                        joint.dampingRatio = 1f;

                        FixedJoint2D joint1 = gameObject.AddComponent<FixedJoint2D>();
                        joint1.connectedBody = peer.GetComponent<Rigidbody2D>();
                        joint1.autoConfigureConnectedAnchor = false;
                        joint1.enableCollision = false;
                        joint1.dampingRatio = 1f;
                    }

                    isBonded = true;
                    peer.GetComponent<AtomController>().isBonded = true;

                    if (collider.tag == "PeakDB")
                    {
                        BondEnergy = BEV.bondEnergyArray[EnergyMatrixPosition + 3,
                                                         peer.GetComponent<AtomController>().EnergyMatrixPosition + 3];
                        BondingOpportunities -= 2;
                        peer.GetComponent<AtomController>().BondingOpportunities -= 2;
                    }
                    else
                    {
                        BondEnergy = BEV.bondEnergyArray[EnergyMatrixPosition,
                                                         peer.GetComponent<AtomController>().EnergyMatrixPosition];
                        BondingOpportunities--;
                        peer.GetComponent<AtomController>().BondingOpportunities--;
                    }

                    CD.JDC.GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, BondEnergy, PVID, 0);
                }
            }
            else
            {
                CollisionEventRegistered = true;
                if (peer.GetComponent<AtomController>().CollisionEventRegistered)
                {
                    FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                    joint.connectedBody = peer.GetComponent<Rigidbody2D>();

                    FixedJoint2D joint1 = peer.AddComponent<FixedJoint2D>();
                    joint1.connectedBody = gameObject.GetComponent<Rigidbody2D>();

                    isBonded = true;
                    peer.GetComponent<AtomController>().isBonded = true;

                    if (collider.tag == "PeakDB")
                    {
                        BondingOpportunities -= 2;
                        peer.GetComponent<AtomController>().BondingOpportunities -= 2;
                    }
                    else
                    {
                        BondingOpportunities--;
                        peer.GetComponent<AtomController>().BondingOpportunities--;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (UnbondingScript2.DontBondAgain > 0)
        {
            UnbondingScript2.DontBondAgain--;
        }

        CollisionEventRegistered = false;
        if (PV.IsMine == true)
            return;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    //public void GetConnections()
    //{
    //    if (CD.GSC.CurrentUnbonding.Contains(gameObject))
    //    {
    //        return;
    //    }

    //    CD.GSC.CurrentUnbonding.Add(gameObject);
    //    FixedJoint2D[] connections = GetComponents<FixedJoint2D>();
    //    foreach (FixedJoint2D connection in connections)
    //    {
    //        connection.connectedBody.gameObject.GetComponent<AtomController>().GetConnections();
    //    }
    //}
}
