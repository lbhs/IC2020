using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cubeScript : MonoBehaviour
{

    public Vector3 velocity;
    public int velocitySpeedUp;
	private Rigidbody rb;
	public int temp;
	private Slider temperatureSlider;
    // Start is called before the first frame update
    void OnEnable()
    {
		temperatureSlider = GameObject.Find("temperature").GetComponent<Slider>();
		
		rb = gameObject.GetComponent<Rigidbody>();
		
        float vx = UnityEngine.Random.Range(-5, 6);
        float vy = Mathf.Sqrt(50 - (vx * vx));        
      
        velocity = new Vector3(vx, vy, 0);

        rb.velocity = velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		temp = (int)temperatureSlider.value;
        if(rb.velocity.sqrMagnitude < 50)
        {
            //print("old velocity =" + velocity);
            rb.velocity = rb.velocity.normalized * 5 * Mathf.Sqrt(2);
            //print("new velocity =" + velocity);
        }
		Debug.Log(temp);
    }
}

