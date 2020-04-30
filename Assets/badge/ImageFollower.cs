using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImageFollower : MonoBehaviour
{
    public GameObject objectToFollow;
    public GameObject CanvasPrefab;
    private GameObject lableCanvas;

    void Start()
    {
        lableCanvas = GameObject.Find("LabelCanvas");
        if(lableCanvas == null)
        {
            lableCanvas = Instantiate(CanvasPrefab);
            lableCanvas.name = "LabelCanvas";
            lableCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        }
        gameObject.transform.SetParent(lableCanvas.transform);
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

}