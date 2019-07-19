using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int NumberOfZoomLevels;
    //private int min = 1;
    //public float size;

   // void update()
   // {
        //size = Camera.main.GetComponent<Camera>().orthographicSize;
   // }
    
    public void ZoomOut()
    {
       if (Camera.main.GetComponent<Camera>().orthographicSize < NumberOfZoomLevels * 8)
        {
            Camera.main.GetComponent<Camera>().orthographicSize += 8;
        }
    }

    public void ZoomIn()
    {
        if (Camera.main.GetComponent<Camera>().orthographicSize > 8 /*min*8*/)
        {
            Camera.main.GetComponent<Camera>().orthographicSize -= 8;
        }
    }
}
