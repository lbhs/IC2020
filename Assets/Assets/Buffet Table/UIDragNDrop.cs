﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("What is spawning (Only check one)")]
    public bool useAddSphere;
    public bool useAddWater;
    [Header(" ")]
    [Header("Settings for addSphere")]
    public float mass;
    public int charge;
    public bool elastic;
    public Color color;
    public float scale;
    [Header("  ")]
    [Header("Other")]
    [Header("No Settings for addWater")]
    
    //[Header("Other")]
    public bool UseingMe;
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        UseingMe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        UseingMe = false;
    }
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        num = int.Parse(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}