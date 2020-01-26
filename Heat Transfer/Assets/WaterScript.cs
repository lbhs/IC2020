using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterScript : MonoBehaviour
{

	public Vector3 velocity;
	public float temp;
	private Rigidbody cube;
	private Slider temperatureSlider;
	
	// Start is called before the first frame update
	void OnEnable()
	{
		cube = gameObject.GetComponent<Rigidbody>();
		temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
		
		float vx = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vx) && (vx < 1))
			{ vx = 2;}

		float vy = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vy) && (vy < 1))
			{ vy = 2;}

		velocity = new Vector3(vx, vy, 0);
		cube.velocity = velocity.normalized*10;
		temp = temperatureSlider.value;
		
		GameObject.Find("GameObject").GetComponent<forces>().nonObjects.Add(gameObject);
	}		
	// Update is called once per frame
	void FixedUpdate()
	{
		temp = temperatureSlider.value;
		if (cube.velocity.sqrMagnitude < 0.002f)
		{
			cube.velocity = new Vector3(1,1,0);
		}

		if (Time.timeScale != 0 && GameObject.Find("GameObject").GetComponent<forces>().recording && cube.velocity.sqrMagnitude < (40 * temp))
		{
			cube.velocity *= 1.3f;
			//print("new velocity = " + velocity.magnitude);
		}
	}
}
