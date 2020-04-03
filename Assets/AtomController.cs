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

    public List<GameObject> variants = new List<GameObject>();
    private int variantCounter;
    public bool isBonded = false;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();
    }

    void OnMouseDown()
    {
        if (PV.IsMine == false)
            return;
        if (GameObject.Find("TurnScreen") == null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                float deg;
                //   if (isBonded == true)
                //  deg = 90f;
                //else
                //   deg = 45f;
                if (isBonded != true)
                    deg = 90f;
                else
                    deg = 0f;
                GetComponent<Rigidbody2D>().rotation -= deg;
                GetComponent<Rigidbody2D>().rotation = Mathf.Round (GetComponent<Rigidbody2D>().rotation);
                //GetComponent<Rigidbody2D>().MoveRotation( transform.rotation.z + -45 );
                //transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -45);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                if (isBonded == false)
                {
                    PV.RPC("RotateGO", RpcTarget.All);
                }
            }
            else
            {
                mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                // Store offset = gameobject world pos - mouse world pos
                mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
            }
        }

    }

    public void BondingFunction(Collider2D collision)
    {
        FixedJoint2D joint;
        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = collision.transform.root.gameObject.GetComponent<Rigidbody2D>();
        joint.enableCollision = false;
    }

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        if (GetComponent<FixedJoint2D>() == null)
        {
            isBonded = false;
        }
        else
        {
            isBonded = true;
        }

        if (PV.IsMine == true)
            return;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
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

    [PunRPC]
    public void RotateGO()
    {
        variants[variantCounter].SetActive(false);
        variantCounter++;
        if (variantCounter >= variants.Count)
        {
            variantCounter = 0;
        }
        variants[variantCounter].SetActive(true);
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
}