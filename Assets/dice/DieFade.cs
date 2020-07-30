using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieFade : MonoBehaviour
{	
	private Color initialColor;
	private Color finalColor;
	private MeshRenderer mesh;
	public float timeToFade;

	void Start()
	{
		mesh = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
		initialColor = mesh.material.color;
		finalColor = mesh.material.color;
		finalColor.a = 0;
	}
	
	public void startFade()
	{
		StartCoroutine(Lerp_MeshRenderer_Color(mesh, timeToFade, initialColor, finalColor));
	}
	
	private IEnumerator Lerp_MeshRenderer_Color(MeshRenderer target_MeshRender, float lerpDuration, Color startLerp, Color targetLerp)
	{
        yield return new WaitForSeconds(1);
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
		DieScript.rolling++;
		GetComponent<DieScript>().Reset();
		mesh.material.color = initialColor;
		yield break;
	}
}
