using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RotateIt : MonoBehaviour
{ 
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

#if UNITY_ANDROID
    private float timeInitial;
    private float minTime = 0.25f;
    private float maxTime = 0.5f;
    private Vector3 initialPos;
    private float distanceThreshold = 0.2f;

    private void OnMouseDown()
    {
        timeInitial = Time.time;
        initialPos = transform.position;
    }

    private void OnMouseUp()
    {
        float timeClicked = Time.time - timeInitial;
        float distanceMoved = Vector3.Distance(transform.position, initialPos);
        if (timeClicked > minTime 
            && timeClicked < maxTime 
            && distanceMoved < distanceThreshold
            && !gameObject.GetComponent<AtomController>().isBonded
            && gameObject.GetComponent<PhotonView>().IsMine)
        {
            transform.Rotate(0f, 0f, 90f);
        }
    }
#endif
}