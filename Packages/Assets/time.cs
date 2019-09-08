using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class time : MonoBehaviour
{
	Text speed;
	
	public void updateTimeScale(float scale)
	{
		GameObject.Find("speedText").GetComponent<Text>().text = "Simulation: " + (scale/4).ToString() + "x";
		if(scale < 0)
		{
			if(GameObject.Find("GameObject").GetComponent<TimeBody>().frame == -1)
			{
				Time.timeScale = 0f;
				GetComponent<UnityEngine.UI.Slider>().value = 0;
			}
			else
			{
				GameObject.Find("GameObject").GetComponent<forces>().startRewind();
				Time.timeScale = Math.Abs(scale/4);
				GetComponent<UnityEngine.UI.Slider>().value = scale;
			}
		}
		else
		{
			Time.timeScale = scale/4;
			GetComponent<UnityEngine.UI.Slider>().value = scale;
			GameObject.Find("GameObject").GetComponent<forces>().stopRewind();
		}
	}
}
