using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterGasScript : MonoBehaviour
{

	public Vector3 velocity;
	public float temp;
	private Rigidbody cube;
	private Slider temperatureSlider;
	
	// Start is called before the first frame update
	void OnEnable()
	{
		cube = gameObject.GetComponent<Rigidbody>();
		temperatureSlider = GameObject.Find("temperatureSlider2").GetComponent<Slider>();
		
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
		if ((cube.velocity.y < 1.0f)&&(cube.velocity.y > -1.0f))
		{
			//water molecules have no minimum velocity!
            //print("particle y velocity low" + cube.velocity.y);
			//cube.AddForce(0,200,0);
		}
		
		if ((cube.velocity.x < 1.0f)&&(cube.velocity.x > -1.0f))
		{
			//print("particle x velocity low" + cube.velocity.x);
			//cube.AddForce(200,0,0);
		}

		if (Time.timeScale != 0 && GameObject.Find("GameObject").GetComponent<forces>().recording && cube.velocity.sqrMagnitude < (20 * (temp-1)))
		{
			cube.velocity *= 1.3f;
			//print("new velocity = " + velocity.magnitude);
		}
	}
}

