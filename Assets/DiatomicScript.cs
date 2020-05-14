using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DiatomicScript : MonoBehaviour
{
    public GameObject DisassociationProduct;
    public int DisassociationEnergy;

    private float mZCoord;
    private Vector3 mOffset;

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (gameObject.GetComponent<PhotonView>().IsMine == false)
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

    void OnMouseDrag()
    {
        if (gameObject.GetComponent<PhotonView>().IsMine == false)
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
}
