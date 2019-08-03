using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class time : MonoBehaviour
{
	public float timespeed;
	
	public void updateTimeScale(float scale)
	{
		timespeed = scale/4;
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
