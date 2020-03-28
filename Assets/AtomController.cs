using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AtomController : MonoBehaviourPunCallbacks
{
    private Vector3 mOffset;
    private float mZCoord;
    private GameObject TurnScreen;
    private PhotonView PV;

    public List<GameObject> variants = new List<GameObject>();
    private int variantCounter;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (GameObject.Find("TurnScreen") != null)
        {
            TurnScreen = GameObject.Find("TurnScreen");
        }
    }

    void OnMouseDown()
    {
        if (PV.IsMine == false)
            return;
        if (TurnScreen == null)
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

    void OnMouseDrag()
    {
        if (PV.IsMine == false)
            return;
        if (TurnScreen == null)
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