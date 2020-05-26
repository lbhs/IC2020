using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RotateIt : MonoBehaviour
{

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
           
    }

    private void OnMouseOver()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(1)
                && !gameObject.GetComponent<AtomController>().isBonded
                && gameObject.GetComponent<PhotonView>().IsMine)
            {
                transform.Rotate(0f, 0f, 90f);
            }
        }
    }
}
