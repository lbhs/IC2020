using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragJoule : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool returnToZero = false; //default value is false
    //public GameObject PrefabToSpawn;
    //private Vector3 prefabWorldPosition;

    public bool UseingMe;

    public void OnDrag(PointerEventData eventData)
    {
        if(JewelMover.JewelsInMotion == true)
        {
            return;
        }
        else
        {
            transform.position = Input.mousePosition;
            UseingMe = true;
        }
        
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
