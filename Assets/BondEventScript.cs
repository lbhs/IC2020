using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondEventScript : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject TwinColider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTriggered = true;
        if (TwinColider.GetComponent<BondEventScript>().isTriggered == true)
        {
            transform.root.GetComponent<AtomController>().BondingFunction(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTriggered = false;
    }
}
