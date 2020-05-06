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

    public int MoleculeElementsIdx;

    public int SingleBondingOpportunities;

    public int EnergyMatrixPosition;

    public int CurrentSingleBondingOpportunities;

    private BondEnergyValues BEV;

    public bool CollisionEventRegistered;

    int PVID;

    private void Start()
    {
        CollisionEventRegistered = false;
        CurrentSingleBondingOpportunities = SingleBondingOpportunities;
        PV = GetComponent<PhotonView>();
        PVID = PV.ViewID;
    }

    void Awake()
    {
        GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();

        BEV = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>();
    }

    void OnMouseDown()
    {
        if (PV.IsMine == false)
            return;
        else
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }

    //public void EvaluatePoints()
    //{
    //    int BondsMade = 0;
    //    foreach (Transform child in transform.GetChild(variantCounter))
    //    {
    //        if (child.gameObject.GetComponent<BondEventScript>() != null)
    //        {
    //            if (child.gameObject.GetComponent<BondEventScript>().Bonded)
    //            {
    //                BondsMade++;
    //            }
    //        }
    //    }
    //    CurrentSingleBondingOpportunities = SingleBondingOpportunities - BondsMade;
    //}

    //public void BondingFunction(Collider2D collision)
    //{
    //    FixedJoint2D joint;
    //    joint = gameObject.AddComponent<FixedJoint2D>();
    //    joint.connectedBody = collision.transform.root.gameObject.GetComponent<Rigidbody2D>();
    //    joint.enableCollision = false;
       
    //    EvaluatePoints();
    //    collision.transform.root.GetComponent<AtomController>().EvaluatePoints();

    //    int BondScore = BEV.bondEnergyArray[EnergyMatrixPosition,
    //                                        collision.transform.root.GetComponent<AtomController>().EnergyMatrixPosition];
    //    int TotalBonusScore = GSC.TotalBonusPoints(transform.root.gameObject);
    //    GSC.GetComponent<PhotonView>().RPC("ChangeScoreUniformly", RpcTarget.All, BondScore, TotalBonusScore);
    //}

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        if (GetComponent<FixedJoint2D>() != null)
        {
            isBonded = true;
        }

        if (PV.IsMine == true)
            return;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
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
                //transform.position = GetMouseAsWorldPoint() + mOffset;
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
            CollisionEventRegistered = true;
            GameObject peer = collider.transform.root.gameObject;
            if (peer.GetComponent<AtomController>().CollisionEventRegistered)
            {
                if (MoleculeElementsIdx == 0 && peer.GetComponent<AtomController>().MoleculeElementsIdx == 0)
                {
                    GSC.GetComponent<PhotonView>().RPC("AddToList", RpcTarget.All, PVID);
                    peer.GetComponent<AtomController>().MoleculeElementsIdx = MoleculeElementsIdx;
                }

                else if (MoleculeElementsIdx == 0 && peer.GetComponent<AtomController>().MoleculeElementsIdx > 0)
                {
                    MoleculeElementsIdx = peer.GetComponent<AtomController>().MoleculeElementsIdx;
                }

                else if (MoleculeElementsIdx > 0 && peer.GetComponent<AtomController>().MoleculeElementsIdx == 0)
                {
                    peer.GetComponent<AtomController>().MoleculeElementsIdx = MoleculeElementsIdx;
                }

                else
                {
                    GSC.GetComponent<PhotonView>().RPC("AssignNewID", RpcTarget.All, MoleculeElementsIdx, peer.GetComponent<AtomController>().MoleculeElementsIdx);
                }

                FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                joint.connectedBody = peer.GetComponent<Rigidbody2D>();

                peer.GetComponent<AtomController>().isBonded = true;
            }
        }
    }

    private void FixedUpdate()
    {
        CollisionEventRegistered = false;
    }
}