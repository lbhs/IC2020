using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasFloat : MonoBehaviour
{
    public Vector3 velocity;
    public float temp;
	private Rigidbody gas;
	private Slider temperatureSlider;

    
    // Start is called before the first frame update
    void Start()
    {
        gas = gameObject.GetComponent<Rigidbody>();
		temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
        gas.velocity = new Vector3(0,4,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
