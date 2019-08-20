
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int NumberOfZoomLevels;
    public GameObject cubeUp;
    public GameObject cubeDown;
    public GameObject cubeLeft;
    public GameObject cubeRight;
    private int zoomCounter;
    private bool zoomLocked;
    //private int min = 1;
    //public float size;

     void update()
     {
     //size = Camera.main.GetComponent<Camera>().orthographicSize;
     }
    void Start()
    {
        zoomCounter = 1;
        zoomLocked = false;
        cubeDown.transform.position = Camera.main.ViewportToWorldPoint(new Vector3((Camera.main.rect.xMax / 2f), Camera.main.rect.yMin, 10));
        cubeUp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3((Camera.main.rect.xMax / 2f), Camera.main.rect.yMax, 10));
        cubeLeft.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMin, (Camera.main.rect.yMax / 2f), 10));
        cubeRight.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMax, (Camera.main.rect.yMax / 2f), 10));
    }

    public void ZoomOut()
    {
       if (Camera.main.GetComponent<Camera>().orthographicSize < NumberOfZoomLevels * 8)
       {
            Camera.main.GetComponent<Camera>().orthographicSize += 8;
            if (zoomCounter != NumberOfZoomLevels && zoomLocked != true)
            {
                cubeUp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3((Camera.main.rect.xMax / 2f), Camera.main.rect.yMin, 10));
                cubeDown.transform.position = Camera.main.ViewportToWorldPoint(new Vector3((Camera.main.rect.xMax / 2f), Camera.main.rect.yMax, 10));
                cubeLeft.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMin, (Camera.main.rect.yMax / 2f), 10));
                cubeRight.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Camera.main.rect.xMax, (Camera.main.rect.yMax / 2f), 10));
                zoomCounter++;
                if (zoomCounter == NumberOfZoomLevels)
                {
                    zoomLocked = true;
                }
            }
       }
    }

    public void ZoomIn()
    {
        if (Camera.main.GetComponent<Camera>().orthographicSize > 8 /*min*8*/)
        {
            Camera.main.GetComponent<Camera>().orthographicSize -= 8;
            zoomCounter--;
        }
    }
}
