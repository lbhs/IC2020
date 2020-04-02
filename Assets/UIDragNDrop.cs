using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool returnToZero = false; //default value is false
    public GameObject PrefabToSpawn;

     public bool UseingMe;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        UseingMe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (returnToZero == true)
        {
            transform.localPosition = Vector3.zero;
        }
        UseingMe = false;
    }
}