using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && !gameObject.GetComponent<AtomController>().isBonded)
        {
            transform.Rotate(0, 0, 90);
        }
    }
}
