using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapIt : MonoBehaviour
{
    public GameObject PrefabToBecome;
    private float LastClickTime;
    private float TimeSinceLastClick;
    public const double DoubleClickThreshold = 0.2;
    private static GameObject prefab;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !GetComponent<AtomController>().isBonded)
        {
            TimeSinceLastClick = Time.time - LastClickTime;
            if (TimeSinceLastClick <= DoubleClickThreshold)
            {

            }
        }
    }
}
