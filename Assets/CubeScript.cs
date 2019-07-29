using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    Rigidbody Cube;
    public Vector3 CubeVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Cube = GetComponent<Rigidbody>();
        Cube.velocity = CubeVelocity;
        //Cube.angularVelocity = new Vector3 (0,0,25);  this would make cube spin. . .
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
