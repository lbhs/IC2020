using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatTransfer : MonoBehaviour
{
    public Vector3 velocity;
	private Rigidbody waterMol;
    public float temp;
    
	//private Slider temperatureSlider;

    // Start is called before the first frame update
    void Start()
    {
        waterMol = gameObject.GetComponent<Rigidbody>();
		//temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
		
		float vx = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vx) && (vx < 1))
			{ vx = 2;}

		float vy = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vy) && (vy < 1))
			{ vy = 2;}

		velocity = new Vector3(vx, vy, 0);
		waterMol.velocity = velocity.normalized*5*temp;
        //temp = individual value for each molecule:  values 1-6;
    }

    // Update is called once per frame
    void Update()
    {
        if(waterMol.velocity.sqrMagnitude<60*temp)
        {
            print ("Water Temp" + temp);
            waterMol.velocity *= 1.3f;
        }

    }
}
