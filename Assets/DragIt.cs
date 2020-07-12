using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DragIt : MonoBehaviour
{
    private Rigidbody2D rb;
    private PhotonView PV;

    private Vector3 mOffset;
    private float mZCoord;

    public GameObject UnbondingJoule;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();
    }

    public void OnMouseDown()
    {
        if (PV.IsMine == false)
        {
            return;
        }
        else if (GameSetupContrller.Instance.Unbonding)
        {
            Vector3 NewMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            NewMousePosition.z = 0;
            PhotonNetwork.Instantiate(UnbondingJoule.name, NewMousePosition, Quaternion.identity);
            UnbondingScript2.WaitABit = 8;
            UnbondingScript2.DontBondAgain = 20;
            Debug.Log("Unbonding initiated");
        }
        else
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }

    void OnMouseDrag()
    {
        if (!PV.IsMine || GameSetupContrller.Instance.Unbonding)
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

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }
}