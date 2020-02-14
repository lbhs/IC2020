using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O2GasScript : MonoBehaviour
{

	public Vector3 velocity;
	public float temp;
	private Rigidbody cube;
	private Slider temperatureSlider;
	
	// Start is called before the first frame update
	void OnEnable()
	{
		cube = gameObject.GetComponent<Rigidbody>();
		temperatureSlider = GameObject.Find("temperatureSlider3").GetComponent<Slider>();
		
		float vx = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vx) && (vx < 1))
			{ vx = 2;}

		float vy = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vy) && (vy < 1))
			{ vy = 2;}

		velocity = new Vector3(vx, vy, 0);
		cube.velocity = velocity.normalized*4;
		temp = temperatureSlider.value;
		
		GameObject.Find("GameObject").GetComponent<forces>().nonObjects.Add(gameObject);
	}		
	// Update is called once per frame
	void FixedUpdate()
	{
		temp = temperatureSlider.value;
		if ((cube.velocity.y < 6.0f)&&(cube.velocity.y > -6.0f))
		{
			//print("particle y velocity low" + cube.velocity.y);
			if (cube.velocity.y < 0)
			{
				cube.AddForce(0, -600, 0);
			}
			else
			{
				cube.AddForce(0, 600, 0);
			}
		}
		
		if ((cube.velocity.x < 3.0f)&&(cube.velocity.x > -3.0f))
		{
			//print("particle x velocity low" + cube.velocity.x);
			if (cube.velocity.x <= 0)
			{
				cube.AddForce(-600, 0, 0);
			}
			else
			{
				cube.AddForce(600, 0, 0);
			}
		}

		if (Time.timeScale != 0 && GameObject.Find("GameObject").GetComponent<forces>().recording && cube.velocity.sqrMagnitude < (20 * temp))
		{
			cube.velocity *= 1.3f;
			//print("new velocity = " + velocity.magnitude);
		}
	}
}
