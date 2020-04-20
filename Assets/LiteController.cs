using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiteController : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject TwinColider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((tag == "Peak" && collision.tag == "Valley") || (tag == "Valley" && collision.tag == "Peak"))
        {
            isTriggered = true;
        }
    }
}
