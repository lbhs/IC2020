using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragIt : MonoBehaviour
{
    private Vector2 mousePosition;
    private Vector2 lastMousePosition;

    public void OnMouseDown()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lastMousePosition = mousePosition;
    }
	
	void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		getParent().Translate(mousePosition-lastMousePosition);
		lastMousePosition = mousePosition;
    }
	
	public Transform getParent()
	{
		Transform parent = transform.parent;
		if(parent == null)
		{
			parent = transform;
		}
		return parent;
	}
}