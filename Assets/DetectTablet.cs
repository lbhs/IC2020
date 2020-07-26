using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTablet : MonoBehaviour
{
	public static bool isTablet()
    {
		var aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
		return (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f);
	}
	
	public static float DeviceDiagonalSizeInInches()
	{
		float screenWidth = Screen.width / Screen.dpi;
		float screenHeight = Screen.height / Screen.dpi;
		float diagonalInches = Mathf.Sqrt (Mathf.Pow (screenWidth, 2) + Mathf.Pow (screenHeight, 2));
	 
		return diagonalInches;
	}
}
