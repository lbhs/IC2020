using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFollower : MonoBehaviour
{
    public GameObject sphereToFollow;
    private GameObject lableCanvas;

    void Start()
    {
        lableCanvas = GameObject.Find("Lable Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        //Convert the player's position to the UI space then apply the offset
        transform.position = worldToUISpace(lableCanvas.GetComponent<Canvas>(), sphereToFollow.transform.position);
    }

    private Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

}
