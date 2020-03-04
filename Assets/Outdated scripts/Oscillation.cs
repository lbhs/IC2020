using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oscillation : MonoBehaviour
{
    public float freq;
    public float mag;
    public Rigidbody cube;

    // Start is called before the first frame update
    void Start()
    {
        cube = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        cube.position = new Vector3(mag*Mathf.Sin(freq*(Time.time)),0,0);
    }
}
