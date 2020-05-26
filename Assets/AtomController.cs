using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AtomController : MonoBehaviourPunCallbacks
{
    private Vector3 mOffset;
    private float mZCoord;
    private PhotonView PV;
    private GameSetupContrller GSC;

    public bool isBonded = false;

    public int MoleculeID;

    public int BondingOpportunities;

    public int EnergyMatrixPosition;

    private BondEnergyValues BEV;

    public bool CollisionEventRegistered;

    public int PVID;

    public bool Monovalent;

    private TextController ScoringSystem;

    public Collider2D CorrespondingCollider;

    GameObject peer;

    FixedJoint2D joint;
    FixedJoint2D joint1;

    RelativeJoint2D ExperimentalJoint;

    void Start()
    {
        GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();
        BEV = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>();

        CollisionEventRegistered = false;
        PV = GetComponent<PhotonView>();
        PVID = PV.ViewID;

        if (PV.Owner == PhotonNetwork.PlayerList[0])
        {
            ScoringSystem = GameObject.Find("UI").transform.GetChild(6).GetComponent<TextController>();
        }
        else
        {
            ScoringSystem = GameObject.Find("UI").transform.GetChild(7).GetComponent<TextController>();
        }
    }

    void OnMouseDown()
    {
        if (PV.IsMine == false)
        {
            return;
        }
        else
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }

    private void Update()
    {

    }

    void OnMouseDrag()
    {
        if (PV.IsMine == false)
            return;
        if (GameObject.Find("TurnScreen") == null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                return;
            }
            else
            {
                GetComponent<Rigidbody2D>().MovePosition(GetMouseAsWorldPoint() + mOffset);
            }
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Peak" || collider.tag == "PeakDB")
        {
            peer = collider.transform.root.gameObject;
            if (PV.IsMine)
            {
                CorrespondingCollider = collider;
                CollisionEventRegistered = true;
                if (peer.GetComponent<AtomController>().CollisionEventRegistered)
                {
                    if (MoleculeID == 0 && peer.GetComponent<AtomController>().MoleculeID == 0)
                    {
                        GSC.GetComponent<PhotonView>().RPC("GenerateID", RpcTarget.All, PVID, peer.GetComponent<AtomController>().PVID);
                    }

                    else if (MoleculeID == 0 && peer.GetComponent<AtomController>().MoleculeID > 0)
                    {
                        GSC.GetComponent<PhotonView>().RPC("TransferSingleElement", RpcTarget.All, PVID, peer.GetComponent<AtomController>().MoleculeID);
                    }

                    else if (MoleculeID > 0 && peer.GetComponent<AtomController>().MoleculeID == 0)
                    {
                        GSC.GetComponent<PhotonView>().RPC("TransferSingleElement", RpcTarget.All, peer.GetComponent<AtomController>().PVID, MoleculeID);
                    }

                    else
                    {
                        GSC.GetComponent<PhotonView>().RPC("MergeMoleculeLists", RpcTarget.All, MoleculeID, peer.GetComponent<AtomController>().MoleculeID);
                    }

                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GetComponent<Rigidbody2D>().angularVelocity = 0f;

                    if (Monovalent)
                    {
                        joint = gameObject.AddComponent<FixedJoint2D>();
                        joint.connectedBody = peer.GetComponent<Rigidbody2D>();
                        joint.enableCollision = false;
                    }

                    // The other element solely is monovalent
                    else if (peer.GetComponent<AtomController>().Monovalent)
                    {
                        joint = peer.AddComponent<FixedJoint2D>();
                        joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                        joint.enableCollision = false;
                        joint.dampingRatio = 1f;
                    }

                    else
                    {
                        FixedJoint2D joint = peer.AddComponent<FixedJoint2D>();
                        joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                        joint.enableCollision = false;
                        joint.dampingRatio = 1f;

                        FixedJoint2D joint1 = gameObject.AddComponent<FixedJoint2D>();
                        joint1.connectedBody = peer.GetComponent<Rigidbody2D>();
                        joint1.enableCollision = false;
                        joint1.dampingRatio = 1f;
                    }

                    isBonded = true;
                    peer.GetComponent<AtomController>().isBonded = true;

                    int BondEnergy = 0;

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

                    for (int NumJoules = 0; NumJoules < BondEnergy; NumJoules++)
                    {
                        GSC.SpawnJoule();
                    }

                    int PotentialBonusSize = GSC.ReturnCompletionSize(MoleculeID);
                    int BonusScore = 0;

                    if (PotentialBonusSize != 0)
                    {
                        if (PotentialBonusSize < 6)
                        {
                            BonusScore = 10 * (PotentialBonusSize - 1);
                        }
                        else
                        {
                            BonusScore = 60;
                        }
                    }

                    GSC.GetComponent<PhotonView>().RPC("ChangeScoreUniformly", RpcTarget.All, BondEnergy, BonusScore);
                }
            }
            else
            {
                joint = gameObject.AddComponent<FixedJoint2D>();
                joint.connectedBody = peer.GetComponent<Rigidbody2D>();

                isBonded = true;
                peer.GetComponent<AtomController>().isBonded = true;
            }
        }
    }

    private void FixedUpdate()
    {
        CollisionEventRegistered = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        if (PV.IsMine == true)
            return;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
