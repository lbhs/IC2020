using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LiteController : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject TwinColider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.GetComponent<PhotonView>().Owner == transform.root.GetComponent<PhotonView>().Owner
            && ((gameObject.tag == "Peak" && collision.tag == "Valley") || (gameObject.tag == "Valley" && collision.tag == "Peak")))
            isTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTriggered = false;
    }
}
