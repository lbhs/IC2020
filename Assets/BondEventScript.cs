using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondEventScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.root.GetComponent<AtomController>().BondingFunction(collision);
    }
}
