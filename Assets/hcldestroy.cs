using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hcldestroy : MonoBehaviour
{
	//private Rigidbody HCl;
    // Start is called before the first frame update
    void Start()
    {
       // HCl = gameObject.GetComponent<Rigidbody>();
		Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
