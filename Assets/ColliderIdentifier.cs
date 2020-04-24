using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderIdentifier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (((tag == "Peak" && collider.tag == "Valley") 
            || (tag == "Valley" && collider.tag == "Peak")
            || (tag == "PeakDB" && collider.tag == "ValleyDB")
            || (tag == "ValleyDB" && collider.tag == "PeakDB")))
        {
            while (true)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
                if (hit == collider)
                {
                    Debug.Log("Alignment between " + name + " and " + collider.name);
                    // transform.root.GetComponent<BondMaker>().AlignmentCheck(collider);
                    break;
                }
            }
        }
    }
}
