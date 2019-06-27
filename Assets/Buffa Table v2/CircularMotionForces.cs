using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMotionForces : MonoBehaviour


{
    public Rigidbody Electron;
    //[SerializeField]
    public float strength;
   

    
    // Start is called before the first frame update
    void Start()
    {
        strength = 1;
        Electron = GetComponent<Rigidbody>();
        Electron.velocity = new Vector3(0, 5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Electron.AddForce(-transform.position.x * strength, -transform.position.y, 0 * strength);
    }
}
