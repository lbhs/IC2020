using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class time : MonoBehaviour
{
	public void updateTimeScale(float scale)
	{
		Time.timeScale = Math.Abs(scale/4);
		GetComponent<UnityEngine.UI.Slider>().value = scale;
		if(scale > 0)
		{
			GameObject.Find("GameObject").GetComponent<forces>().stopRewind();
		}
		if(scale < 0)
		{
			GameObject.Find("GameObject").GetComponent<forces>().startRewind();
		}
	}
}
