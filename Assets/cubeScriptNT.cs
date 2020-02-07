using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cubeScriptNT : MonoBehaviour
{

	public Vector3 velocity;
	
	private Rigidbody cube;
	
	
	// Start is called before the first frame update
	void OnEnable()
	{
		cube = gameObject.GetComponent<Rigidbody>();
		
		float vx = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vx) && (vx < 1))
			{ vx = 2;}

		float vy = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vy) && (vy < 1))
			{ vy = 2;}

		velocity = new Vector3(vx, vy, 0);
		cube.velocity = velocity.normalized*4;
		
		
		GameObject.Find("GameObject").GetComponent<forces>().nonObjects.Add(gameObject);
	}		
	// Update is called once per frame
	
    void FixedUpdate()
	{
		//temp = temperatureSlider.value;
		
		if ((cube.velocity.x < 3.0f)&&(cube.velocity.x >= 0))
		{
			//print("particle x velocity low" + cube.velocity.x);
			cube.AddForce(200,0,0);
		}
        
		if ((cube.velocity.x > -3.0f)&&(cube.velocity.x < 0))
		{
			//print("particle x velocity low" + cube.velocity.x);
			cube.AddForce(-200,0,0);
		}
		
		if ((cube.velocity.y < 3.0f)&&(cube.velocity.y >= 0))
		{
			//print("particle y velocity low" + cube.velocity.x);
			cube.AddForce(0,200,0);
		}
        if ((cube.velocity.y > -3.0f)&&(cube.velocity.y < 0))
		{
			//print("particle y velocity low" + cube.velocity.x);
			cube.AddForce(0,-200,0);
		}
		if (Time.timeScale != 0 && GameObject.Find("GameObject").GetComponent<forces>().recording && cube.velocity.sqrMagnitude < 100)
		{
			cube.velocity *= 1.2f;
			//print("new velocity = " + velocity.magnitude);
		}
	}
}
