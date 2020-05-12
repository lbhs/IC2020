using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImageFollower : MonoBehaviour
{

}
/*
{
    //This script is attached to the "Badge" GameObject
    public GameObject objectToFollow;  //This attaches the badge to a specific atom
    public GameObject CanvasPrefab;  //the Badge UI element is on a Canvas
    private GameObject lableCanvas;  //This is the Canvas that the badge resides upon

    void Start()
    {
        lableCanvas = GameObject.Find("LabelCanvas");
        if(lableCanvas == null)
        {
            lableCanvas = Instantiate(CanvasPrefab);
            lableCanvas.name = "LabelCanvas";
            lableCanvas.GetComponent<Canvas>().worldCamera = Camera.main;  //needed to make the badge appear in the right spot
        }
        gameObject.transform.SetParent(lableCanvas.transform);  //Badge is a child of LabelCanvas
    }
    
    public void AddBadge()
    {
        print("got to the image follower script)");
        lableCanvas = GameObject.Find("LabelCanvas");
        if(lableCanvas == null)
        {
            lableCanvas = Instantiate(CanvasPrefab);
            lableCanvas.name = "LabelCanvas";
            lableCanvas.GetComponent<Canvas>().worldCamera = Camera.main;  //needed to make the badge appear in the right spot
        }
        gameObject.transform.SetParent(lableCanvas.transform);  //gameObject = the Badge.  The Badge is a child of LabelCanvas
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToFollow != null)
        {
            //Convert the player's position to the UI space then apply the offset
            transform.position = worldToUISpace(lableCanvas.GetComponent<Canvas>(), objectToFollow.transform.position);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    private Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screen point to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

}*/