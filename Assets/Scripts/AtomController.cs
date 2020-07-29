using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomController : MonoBehaviour, IPunObservable
{
    #region Public Member Variables

    // isBonded: dictates whether the atom can be rotated
    public bool isBonded = false;

    public int MoleculeID;

    // single bond: -1 bonding opportunities
    // double bond: -2 bonding opportunities
    public int BondingOpportunities;

    // Used to calculate bond energy from a matrix of values
    public int EnergyMatrixPosition;

    public bool CollisionEventRegistered;

    // PhotonView ID -- used to retrieve the GameObject during RPC calls
    public int PVID;

    public bool Monovalent;

    #endregion

    #region Private Member Values

    // peer: the GameObject that this current GameObject is bonding with
    private GameObject peer;

    private int BondEnergy;

    private PhotonView PV;

    // Badges are given when a molecule is completed
    private GameObject BadgeRecipient;

    // BadgeData is a scriptable object
    [SerializeField]
    private BadgePrefabs BadgeData;

    #endregion

    void Start()
    {
        CollisionEventRegistered = false;

        PV = GetComponent<PhotonView>();
        PVID = PV.ViewID;

        BondEnergy = 0;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (UnbondingScript2.DontBondAgain > 0)
        {
            Debug.Log("Not ready to bond");
            return;
        }

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
                    GameSoundEffectsController.Instance.BondFormed();

                    #region Handling MoleculeIDs
                    // RPCs are used for the ID methods because these changes should affect both players' ID databases
                    if (MoleculeID == 0 && peer.GetComponent<AtomController>().MoleculeID == 0)
                    {
                        // Create an ID for both atoms
                        MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("GenerateID", RpcTarget.All, PVID, peer.GetComponent<AtomController>().PVID);
                    }

                    else if (MoleculeID == 0 && peer.GetComponent<AtomController>().MoleculeID > 0)
                    {
                        // Give this GameObject the same MoleculeID as the peer
                        MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("TransferSingleElement", RpcTarget.All, PVID, peer.GetComponent<AtomController>().MoleculeID);
                    }

                    else if (MoleculeID > 0 && peer.GetComponent<AtomController>().MoleculeID == 0)
                    {
                        // Give peer the same MoleculeID as this GameObject
                        MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("TransferSingleElement", RpcTarget.All, peer.GetComponent<AtomController>().PVID, MoleculeID);
                    }

                    else
                    {
                        // Give the GameObjects attached to this GameObject the same MoleculeID as peer's atoms
                        MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("MergeMoleculeLists", RpcTarget.All, MoleculeID, peer.GetComponent<AtomController>().MoleculeID);
                    }
                    #endregion

                    #region Bonding Strategies

                    // Optimized bonding
                    if (Monovalent)
                    {
                        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                        joint.connectedBody = peer.GetComponent<Rigidbody2D>();
                        joint.autoConfigureConnectedAnchor = false;
                        joint.enableCollision = false;
                    }

                    // Solely the other element is monovalent
                    else if (peer.GetComponent<AtomController>().Monovalent)
                    {
                        FixedJoint2D joint = peer.AddComponent<FixedJoint2D>();
                        joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                        joint.autoConfigureConnectedAnchor = false;
                        joint.enableCollision = false;
                    }

                    else
                    {
                        FixedJoint2D joint = peer.AddComponent<FixedJoint2D>();
                        joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                        joint.autoConfigureConnectedAnchor = false;
                        joint.enableCollision = false;

                        FixedJoint2D joint1 = gameObject.AddComponent<FixedJoint2D>();
                        joint1.connectedBody = peer.GetComponent<Rigidbody2D>();
                        joint1.autoConfigureConnectedAnchor = false;
                        joint1.enableCollision = false;
                    }

                    isBonded = true;
                    peer.GetComponent<AtomController>().isBonded = true;

                    #endregion

                    #region Obtaining Bond Energy & Modifying the Score

                    if (collider.tag == "PeakDB")
                    {
                        // Formation of a double bond
                        BondEnergy = BondEnergyValues.Instance.ComputeBondEnergy(EnergyMatrixPosition + 3, peer.GetComponent<AtomController>().EnergyMatrixPosition + 3);
                        // There are 2 less bonding opportunities for both this GameObject and its peer
                        BondingOpportunities -= 2;
                        peer.GetComponent<AtomController>().BondingOpportunities -= 2;
                    }
                    else
                    {
                        // Formation of a single bond
                        BondEnergy = BondEnergyValues.Instance.ComputeBondEnergy(EnergyMatrixPosition, peer.GetComponent<AtomController>().EnergyMatrixPosition);
                        // There is 1 less bonding opportunity for both this GameObject and peer
                        BondingOpportunities--;
                        peer.GetComponent<AtomController>().BondingOpportunities--;
                    }

                    // IncrementJDC handles both the joule holder AND scoring
                    JouleDisplayController.Instance.GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, BondEnergy, PVID, MoleculeIDHandler.Instance.ReturnCompletionScore(GetComponent<AtomController>().MoleculeID));

                    #endregion

                    DetermineBadge();
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isBonded);
            stream.SendNext(MoleculeID);
            stream.SendNext(BondingOpportunities);
        }
        else if (stream.IsReading)
        {
            isBonded = (bool)stream.ReceiveNext();
            MoleculeID = (int)stream.ReceiveNext();
            BondingOpportunities = (int)stream.ReceiveNext();
        }
    }

    private void DetermineBadge()
    {
        int CompletionScore = MoleculeIDHandler.Instance.ReturnCompletionScore(MoleculeID);
        if (CompletionScore > 0)
        {
            if (CompletionScore == 10)
            {
                BadgeRecipient = gameObject;
            }
            else
            {
                foreach (GameObject GO in MoleculeIDHandler.Instance.GetElementsAtGivenPosition(MoleculeID))
                {
                    if (GO.tag == "Oxygen")
                    {
                        BadgeRecipient = GO;
                    }

                    if (GO.tag == "Carbon")
                    {
                        BadgeRecipient = GO;
                        // Carbon takes precedence
                        break;
                    }
                }
            }

            PV.RPC("ApplyBadge", RpcTarget.All, (CompletionScore - 10) / 10, BadgeRecipient.GetComponent<AtomController>().PVID);
        }
    }

    [PunRPC]
    private void ApplyBadge(int BadgePrefabIndex, int RecipientPVID)
    {
        GameObject BadgeRecipient = PhotonView.Find(RecipientPVID).gameObject;
        GameObject Badge = Instantiate(BadgeData.Badges[BadgePrefabIndex], BadgeRecipient.transform);
        Badge.transform.localPosition = new Vector3(-1.2f, 1f, -.25f);
        Badge.transform.Rotate(0, 0, Mathf.Abs(BadgeRecipient.transform.rotation.eulerAngles.z));
    }
}
