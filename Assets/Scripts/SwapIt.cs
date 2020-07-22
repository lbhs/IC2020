using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
                if (AtomInventory.Instance.PrefabCanBeDrawn(PrefabToBecome.name, true))
                {
                    // Steps
                    // 1. Instantiate the the new atom (GO)
                    // 2. gameObject.name.Remove(gameObject.name.Length - 7) is the prefab identifier of this GameObject, and since we are about to destroy it, let us add it back to the inventory
                    // 3. Destroy this atom
                    GameObject GO = PhotonNetwork.Instantiate(PrefabToBecome.name, transform.position, Quaternion.identity);
                    AtomInventory.Instance.AddPrefab(gameObject.name.Remove(gameObject.name.Length - 7));
                    PhotonNetwork.Destroy(gameObject);
                }
                else
                {
                    if (PrefabToBecome.GetComponent<SwapIt>().PrefabToBecome.name + "(Clone)" == gameObject.name || gameObject.name == "OxygenEB(Clone)")
                    {
                        ConversationTextDisplayScript.Instance.OutOfInventory3();
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LastClickTime = Time.time;
        }
    }
}
