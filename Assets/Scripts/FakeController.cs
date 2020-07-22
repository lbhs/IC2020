
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FakeController : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;


    public List<GameObject> variants = new List<GameObject>();
    private int variantCounter;

    void Start()
    {
    }

    void OnMouseDown()
    {

      
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -45);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                RotateGO();
            }
            else
            {
                mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                // Store offset = gameobject world pos - mouse world pos
                mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
            }
        

    }

    void OnMouseDrag()
    {

       
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                return;
            }
            else
            {
                transform.position = GetMouseAsWorldPoint() + mOffset;
            }
        
    }

   
    public void RotateGO()
    {
        variants[variantCounter].SetActive(false);
        variantCounter++;
        if (variantCounter >= variants.Count)
        {
            variantCounter = 0;
        }
        variants[variantCounter].SetActive(true);
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