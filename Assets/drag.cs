using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drag : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		
        // Store offset = gameobject world pos - mouse world pos
        // offset allows you to grab the object from the side of the circle, not just the center
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
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

    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
        //Debug.Log(this.transform.position);
    }

    void OnMouseUp()
    {
        
    }

    //updates everyframe
    void Update()
    {
       //like OnMouseDown, but for right-click
       if(Input.GetMouseButtonDown(1))
        {

        }
    }
}