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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);
            if (hit.collider == collider)
            {
                Debug.Log("Aligned with " + hit.collider.name);
            }
            transform.root.GetComponent<BondMaker>().AlignmentCheck(collider);
            CalculateSlopeBetweenCorrespondingElements(collider);
        }
    }

    private void CalculateSlopeBetweenCorrespondingElements(Collider2D corresponding)
    {
        float slope = (transform.position.y - corresponding.transform.position.y)
                        / (transform.position.x - corresponding.transform.position.x);
        Debug.Log(slope + " between " + tag + " and " + corresponding.tag);
    }
}
