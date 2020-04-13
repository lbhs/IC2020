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

    
    
}
