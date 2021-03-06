﻿using System;
using UnityEngine;
using IC2020;

public class Covalent : MonoBehaviour
{
	private forces mainObject;
	//public bool collided;
    // Start is called before the first frame update
    void Start()
    {
        mainObject = GameObject.Find("GameObject").GetComponent<forces>();
    }

    private void OnCollisionEnter(Collision collision)
    {
		Covalent otherP = collision.gameObject.GetComponent<Covalent>();
        if(otherP != null)
		{
			if(collision.gameObject.GetComponent<Renderer>().material.color == ICColor.Hydrogen) //for acid base reactions only
			{
				Destroy(otherP);
				charger charge = GetComponent<charger>();
						
				ConfigurableJoint cjoint;
				cjoint = collision.gameObject.AddComponent<ConfigurableJoint>();
				cjoint.xMotion = ConfigurableJointMotion.Limited;
				cjoint.yMotion = ConfigurableJointMotion.Limited;
				cjoint.zMotion = ConfigurableJointMotion.Locked;
				cjoint.angularXMotion = ConfigurableJointMotion.Limited;
				cjoint.angularYMotion = ConfigurableJointMotion.Limited;
				cjoint.angularZMotion = ConfigurableJointMotion.Locked;
				cjoint.connectedBody = gameObject.GetComponent<Rigidbody>();
				cjoint.anchor = Vector3.down;
				cjoint.angularXMotion = ConfigurableJointMotion.Limited;
				cjoint.angularYMotion = ConfigurableJointMotion.Limited;
				cjoint.angularZMotion = ConfigurableJointMotion.Locked;
				cjoint.autoConfigureConnectedAnchor = false;
				//add rotations based on angle of collision with tangent probably (rotate, connect, then rotate back)
				if(gameObject.GetComponent<Renderer>().material.color == ICColor.Oxygen && charge.charge == -1){
					collision.gameObject.transform.position = gameObject.transform.position + new Vector3(0.5f, -0.6f, 0);
					cjoint.connectedAnchor = new Vector3(0f, 0f, 0);
					Destroy(GetComponent<Covalent>());
				}
				else
				{
					collision.gameObject.transform.position = gameObject.transform.position + new Vector3(0f, 0.750f, 0);
					cjoint.connectedAnchor = new Vector3(0f, 0f, 0);
				}
				
				charge.updateCharge(charge.charge + 1);
				collision.gameObject.GetComponent<charger>().updateCharge(0);
				
				var limit = new SoftJointLimit();
				limit.limit = 0.1f;
				//limit.SoftJointLimitSpring = 40.0f;
				cjoint.linearLimit = limit;

				limit.limit = 10.0f;
				cjoint.angularYLimit = limit;
				cjoint.angularZLimit = limit;
				cjoint.lowAngularXLimit = limit;
				cjoint.highAngularXLimit = limit;
			}
			//for general activity series reactions
			else if(false/*activity series one greater than another*/)
			{
				float tempcharge = GetComponent<charger>().charge;
				GetComponent<charger>().updateCharge(0);
				collision.gameObject.GetComponent<charger>().updateCharge(tempcharge);
			}
		}
	}
}