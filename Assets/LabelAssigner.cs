using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelAssigner : MonoBehaviour
{
    public bool hasFlag = false; 
    private int imgToUse;
    public GameObject tempLable;
	public string Lable;
	public bool useTextLable = true;

    void Start()
    {
		float charge = GetComponent<charger>().charge;
        if (!useTextLable)
        {
            if (gameObject.GetComponent<charger>().charge < 0) imgToUse = 1;
            else imgToUse = 0;
            tempLable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[imgToUse], Vector3.zero, Quaternion.identity);
            tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
            tempLable.GetComponent<ImageFollower>().sphereToFollow = gameObject;
            Debug.Log(imgToUse);
        }
        else
        {
            tempLable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().TextPrefab, Vector3.zero, Quaternion.identity);
            tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
            tempLable.GetComponent<ImageFollower>().sphereToFollow = gameObject;
			if(charge > 0)
			{
				tempLable.GetComponent<Text>().text = Lable + " +";
				/*if(charge != 1)
				{
					tempLable.GetComponent<Text>().text = + charge.ToString();
				*/
            }
			else if(charge < 0)
			{
				tempLable.GetComponent<Text>().text = Lable + " " + charge.ToString();
			}
			else
			{
				tempLable.GetComponent<Text>().text = Lable;
			}
        }
        if (hasFlag)
        {
            tempLable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().FlagPrefab, Vector3.zero, Quaternion.identity);
            tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
            tempLable.GetComponent<ImageFollower>().sphereToFollow = gameObject;
        }
    }
	
	public void updateCharge(float charge)
	{
		if(charge > 0)
		{
			tempLable.GetComponent<Text>().text = Lable + " +" + charge.ToString();
		}
		else if(charge < 0)
		{
			tempLable.GetComponent<Text>().text = Lable + " " + charge.ToString();
		}
		else
		{
			tempLable.GetComponent<Text>().text = Lable;
		}
	}

}
