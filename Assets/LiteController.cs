using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiteController : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject TwinColider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTriggered = false;
    }
}
