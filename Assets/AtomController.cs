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
                transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -45);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                PV.RPC("RotateGO", RpcTarget.All);
            }
            else
            {
                mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                // Store offset = gameobject world pos - mouse world pos
                mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
            }
        }

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
                transform.position = GetMouseAsWorldPoint() + mOffset;
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