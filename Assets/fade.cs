﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fade : MonoBehaviour
{	
	private Color initialColor;
	private Color finalColor;
	private MeshRenderer mesh;
	public float timeTillFade;
	public float timeToFade;

	void Start()
	{
		mesh = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
		initialColor = mesh.material.color;
		finalColor = mesh.material.color;
		finalColor.a = 0;
	}

    public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(Lerp_MeshRenderer_Color(mesh, timeToFade, initialColor, finalColor));
		}
	}
	
	private IEnumerator Lerp_MeshRenderer_Color(MeshRenderer target_MeshRender, float lerpDuration, Color startLerp, Color targetLerp)
	{
		yield return new WaitForSeconds(timeTillFade);
		float lerpStart_Time = Time.time;
		float lerpProgress;
		bool lerping = true;
		while (lerping)
		{
			yield return new WaitForEndOfFrame();
			lerpProgress = Time.time - lerpStart_Time;
			if (target_MeshRender != null)
			{
				target_MeshRender.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
			}
			else
			{
				lerping = false;
			}
			
			
			if (lerpProgress >= lerpDuration)
			{
				lerping = false;
			}
		}
		GetComponent<DiceScript>().Reset();
		mesh.material.color = initialColor;
		yield break;
	}
}
