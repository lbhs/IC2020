using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyTotal : MonoBehaviour
{
    static int leftTotal;
	static int middleTotal;
	static int rightTotal;
	static int totalTotal;

    // Update is called once per frame
    void FixedUpdate()
    {
		leftTotal = 0;
		middleTotal = 0;
		rightTotal = 0;
		totalTotal = 0;
		
		foreach(Collider obj in Physics.OverlapSphere(new Vector3(2,0,0), 15))
		{
			if(obj.gameObject.GetComponent<IndividMolVelocity>() != null)
			{
				if(obj.gameObject.transform.position.x < 2f)
				{
					leftTotal += obj.gameObject.GetComponent<IndividMolVelocity>().temp;
				}
				else if(obj.gameObject.transform.position.x > 2f)
				{
					rightTotal += obj.gameObject.GetComponent<IndividMolVelocity>().temp;
				}
				totalTotal += obj.gameObject.GetComponent<IndividMolVelocity>().temp;
			}
			else if(obj.gameObject.GetComponent<HeatTransfer2>() != null)
			{
				middleTotal += obj.gameObject.GetComponent<HeatTransfer2>().BoxTemp;
				totalTotal += obj.gameObject.GetComponent<HeatTransfer2>().BoxTemp;
			}
		}
		
		GameObject.Find("leftText").GetComponent<Text>().text = "Left Energy: " + leftTotal.ToString() + " Joules";
		GameObject.Find("rightText").GetComponent<Text>().text = "Right Energy: " + rightTotal.ToString() + " Joules";
		GameObject.Find("middleText").GetComponent<Text>().text = "Energy in Barrier: " + middleTotal.ToString() + " Joules";
		GameObject.Find("totalText").GetComponent<Text>().text = "Total Energy: " + totalTotal.ToString() + " Joules";
    }
}
