using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cubeScript : MonoBehaviour
{

	public Vector3 velocity;
	public float temp;
	private Rigidbody cube;
	private Slider temperatureSlider;
	
	// Start is called before the first frame update
	void OnEnable()
	{
		cube = gameObject.GetComponent<Rigidbody>();
		//no temperatureSlider in Exothermic rxn sim
		//temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
		temp = 5;  //this is the initial temp of H+ ions in the simulation

		float vx = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vx) && (vx < 1))
			{ vx = 2;}

		float vy = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vy) && (vy < 1))
			{ vy = 2;}

		velocity = new Vector3(vx, vy, 0);
		cube.velocity = velocity.normalized*temp;
		//temp = temperatureSlider.value;
		print("cubetemp" + temp);
		
		GameObject.Find("GameObject").GetComponent<forces>().nonObjects.Add(gameObject);
	}		
	// Update is called once per frame
	void FixedUpdate()
	{
		//temp = temperatureSlider.value;
		if ((cube.velocity.x < 3.0f)&&(cube.velocity.x > 0))
		{
			//print("particle x velocity low" + cube.velocity.x);
			cube.AddForce(400,0,0);
		}
        if ((cube.velocity.x > -3.0f)&&(cube.velocity.x < 0))
		{
			//print("particle x velocity low" + cube.velocity.x);
			cube.AddForce(-400,0,0);
		}

		if (Time.timeScale != 0 && GameObject.Find("GameObject").GetComponent<forces>().recording && cube.velocity.sqrMagnitude < (30 * temp))
		{
			cube.velocity *= 1.3f;
			//print("new velocity = " + velocity.magnitude);
		}
	}
}
