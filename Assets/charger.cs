using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charger : MonoBehaviour
{
	/*
	-Simply gives charge variable to individual particles
	-To use, add this script as a component to any game object and make sure that forces.cs is running
	*/
    public float charge;
	
	public void updateCharge(float newcharge)
	{
		charge = newcharge;
		GetComponent<LabelAssigner>().updateCharge(newcharge);
	}
}
