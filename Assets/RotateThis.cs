using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThis : MonoBehaviour
{
    //private gameObject bondCheck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1) && gameObject.GetComponent<BondMaker>().bonded == false)
        {
            transform.Rotate(0,0,90);
        }
        
        
    }

#if UNITY_IPHONE || UNITY_Android
    private float TimeInital;
    private float minTime = 0.5f;
    private float maxTime = 1f;
    private Vector3 InitalPos;
    private float DistanceThreshold = 0.2f;
    private void OnMouseDown()
    {
        TimeInital = Time.time;
        InitalPos = transform.position;
    }
    private void OnMouseUp()
    {
        float ClickTime = Time.time - TimeInital;
        float distance = Vector3.Distance(InitalPos, transform.position);
        if (ClickTime > minTime && ClickTime < maxTime && distance < DistanceThreshold &&gameObject.GetComponent<BondMaker>().bonded == false)
        {
            transform.Rotate(0, 0, 90);
        }
    }
#endif

}
