using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time : MonoBehaviour
{

	public void updateTimeScale(float scale)
	{
		Time.timeScale = scale/4;
	}
}
