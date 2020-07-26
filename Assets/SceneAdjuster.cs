using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneAdjuster : MonoBehaviour
{
    void Start()
    {
        if(DetectTablet.isTablet())
		{
			Debug.Log("adjusting scene for tablet");
			GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = 8;
			GameObject.Find("die_object").transform.position = new Vector3(1f, -9f, -2.5f);
			GameObject.Find("right wall").transform.position = new Vector3(14.693f, 0f, 0f);
			GameObject.Find("left wall").transform.position = new Vector3(-9.415f, 0f, 0f);
			GameObject.Find("lower wall").transform.position = new Vector3(0f, -7.01f, 1f);
			GameObject.Find("upper wall").transform.position = new Vector3(0f, 8.48f, 0f);
			GameObject.Find("LeftWall").transform.position = new Vector3(-16.84f, 0.28f, -3.58702015f);
			GameObject.Find("TopWall").transform.position = new Vector3(-0.7f, 27.8f, -3.65244625f);
			GameObject.Find("RightWall").transform.position = new Vector3(24.34f, 5.2f, -3.749687f);
			GameObject.Find("BottomWall").transform.position = new Vector3(0.5479321f, -22.3f, -3.683281f);
		}
    }
}
