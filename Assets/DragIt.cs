using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragIt : MonoBehaviour
{
    private bool isDragging;
    private Vector2 mousePosition;
    private Vector2 MovePosition;
    private float deltaX;
    private float deltaY;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMouseDown()
    {
        
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        deltaX = mousePosition.x - transform.position.x;
        deltaY = mousePosition.y - transform.position.y;
        
    }

    public void OnMouseUp()
    {
        
    }

   
    
    void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(new Vector2(mousePosition.x-deltaX, mousePosition.y-deltaY));
        //transform.position = new Vector2(mousePosition.x-deltaX, mousePosition.y-deltaY);
		
    }

    // Start is called before the first frame update
    


    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }
}
