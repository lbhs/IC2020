using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testscript : MonoBehaviour
{
	 public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
         rb = GetComponent<Rigidbody>();

        // Moves the GameObject using it's transform.
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
         // Moves the GameObject to the left of the origin.
        if (transform.position.x > 3.0f)
        {
            transform.position = new Vector3(-3.0f, 0.0f, 0.0f);
        }

        rb.MovePosition(transform.position + transform.right * Time.fixedDeltaTime);
    }
}
