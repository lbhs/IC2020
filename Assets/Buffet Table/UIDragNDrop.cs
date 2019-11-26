using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [HideInInspector] public bool isWildCard; //variable to know if this tile should be a wild card
    [HideInInspector] public bool useAddShpere = false;
    [HideInInspector] public bool isInteractable = true;
    //all the variables determining what gets spawned
    [HideInInspector] public string particleName;
    [HideInInspector] public float charge;
    [HideInInspector] public Color color;
    [HideInInspector] public float mass;
    [HideInInspector] public float scale;
    [HideInInspector] public float bounciness;
    [HideInInspector] public bool precipitate;
    [HideInInspector] public float friction;

    // usingMe is set to true whenever the UI element containing the UIDragNDrop is being dragged.
    [HideInInspector] public bool UseingMe;
    public void OnDrag(PointerEventData eventData)
    {
        // Sets usingMe to true when dragging.
        if (isInteractable)
        {
            transform.position = Input.mousePosition; // Makes the image follow the mouse.
            UseingMe = true;
        }
        //WildCardMenu.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // sets usingMe to false when dropped.

        transform.localPosition = Vector3.zero; // Resets the image's position to the buffet table.
        UseingMe = false;

        //Debug.Log("yep");
    }
    [HideInInspector] public int num;
    private GameObject WildCardMenu;
    private GameObject RightClickMenu;

    void Awake()
    {
        WildCardMenu = GameObject.Find("WildCardMenu");
        RightClickMenu = GameObject.Find("Right-Click Menu");
    }

    void Start()
    {
        //undo the comment below to enable wild card. also line 31 in OnDrag function
        //WildCardMenu.SetActive(false);
        num = int.Parse(gameObject.name);
    }

    private void Update()
    {
        //if this tile was right clicked
        if (RectTransformUtility.RectangleContainsScreenPoint(gameObject.transform.parent.GetComponent<RectTransform>(), Input.mousePosition) && Input.GetMouseButtonDown(1) && isWildCard == true)
        {

            WildCardMenu.SetActive(true);
            RightClickMenu.SetActive(false);
            WildCardMenu.GetComponent<WildCardController>().currentTile = gameObject;
            WildCardMenu.GetComponent<WildCardController>().ReverseUpdateWildMenu();
            //Debug.Log(gameObject.name);
            //WildCardMenu.GetComponent<WildCardController>().updateWildMenu();
        }
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