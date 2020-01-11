using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    //These variable and other commented out code in this script, some code in the UIDropToWorld script,
    //code in the start function and a big chuck in the update of DragNDrop script,
    //as well as the PrefabButtonSetter, ParticleList, WildCardController, RightClickHelper scrips,
    //and the two GameObjects right-click-canvas and WildCardMenu (both disabled) 
    //are all apart of the new method to control the buffet table in a better and more dynamic way.
    //These features were disabled because they were buggy and incomplete and because we had bigger fish to fry.
    //For now, refer to the video on the team Google drive to see how to use the buffet table.
    /*[HideInInspector] public bool isWildCard; //variable to know if this tile should be a wild card
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
    

    [HideInInspector] public int num;
    void Start()
    {
        num = int.Parse(gameObject.name);
        //WildCardMenu.SetActive(false);
    }*/

    // usingMe is set to true whenever the UI element containing the UIDragNDrop is being dragged.
    [HideInInspector] public bool UseingMe;
    public void OnDrag(PointerEventData eventData)
    {
        //if (isInteractable)
        //{
            transform.position = Input.mousePosition; // Makes the image follow the mouse.
            UseingMe = true;// Sets usingMe to true when dragging.
        //}
        //WildCardMenu.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero; // Resets the image's position to the buffet table.
        UseingMe = false; // sets usingMe to false when dropped.
    }







        //private GameObject WildCardMenu;
        //private GameObject RightClickMenu;
        /*
        void Awake()
        {
            //WildCardMenu = GameObject.Find("WildCardMenu");
            //RightClickMenu = GameObject.Find("Right-Click Menu");
        }*/


        /*
        private void Update()
        {
            //if this tile was right clicked
            if (RectTransformUtility.RectangleContainsScreenPoint(gameObject.transform.parent.GetComponent<RectTransform>(), Input.mousePosition) && Input.GetMouseButtonDown(1) && isWildCard == true)
            {

                WildCardMenu.SetActive(true);
                RightClickMenu.SetActive(false);
                WildCardMenu.GetComponent<WildCardController>().currentTile = gameObject;
            }
        }*/

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