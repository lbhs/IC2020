﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class DragJoule : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool returnToZero = false; //default value is false
    //public GameObject PrefabToSpawn;
    //private Vector3 prefabWorldPosition;

    public bool UseingMe;

    public void OnDrag(PointerEventData eventData)
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            transform.position = Input.mousePosition;
            UseingMe = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (returnToZero == true)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
        UseingMe = false;
        
    }
}
