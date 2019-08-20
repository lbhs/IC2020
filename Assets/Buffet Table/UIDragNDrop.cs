using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    // usingMe is set to true whenever the UI element containing the UIDragNDrop is being dragged.
    public bool UseingMe;
    public void OnDrag(PointerEventData eventData)
    {
        // Sets UseingMe to true when dragging.
        
        transform.position = Input.mousePosition; // Makes the image follow the mouse.
        UseingMe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // sets UseingMe to false when dropped.
        
        transform.localPosition = Vector3.zero; // Resets the image's position to the buffet table.
        UseingMe = false;
    }
    public int num;
    
    void Start()
    {
        num = int.Parse(gameObject.name);
    }
}

/*
    [Header("What is spawning (Only check one)")]
    public bool useAddSphere;
    public bool useAddWater;
    [Header(" ")]
    [Header("Settings for addSphere")]
    public float mass;
    public float charge;
    public Color color;
    public float scale;
    public float bounciness;
    public int ImageToUse;
    [Header("  ")]
    [Header("Other")]
    [Header("No Settings for addWater")]*/
    
//[Header("Other")]