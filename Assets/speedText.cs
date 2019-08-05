using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speedText : MonoBehaviour
{
	Text speed;
	// Start is called before the first frame update
	void Start()
	{
		speed = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update()
	{
		speed.text = "Speed: " + GameObject.Find("Slider").GetComponent<time>().timespeed.ToString() + "x";
	}
}
