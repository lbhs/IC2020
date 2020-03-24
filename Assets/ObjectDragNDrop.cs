using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectDragNDrop : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private GameObject TurnScreen;

    void Update()
    {
        TurnScreen = GameObject.Find("It's Not Your Turn Screen");
    }

    void OnMouseDown()
    {
        if (TurnScreen == null)
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }

    void OnMouseDrag()
    {
        if (TurnScreen == null)
        {
            transform.position = GetMouseAsWorldPoint() + mOffset;
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}