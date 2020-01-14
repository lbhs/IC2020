using System;
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
			/* good code probably useful for general covalent
			if(otherP.collided)
			{
				otherP.collided = false;
			}
			else
			{
				collided = true;
			*/
			if(gameObject.GetComponent<Renderer>().material.color == ICColor.Oxygen) //temp check
			{
				Destroy(otherP);
				charger charge1 = GetComponent<charger>();
				charger charge2 = collision.gameObject.GetComponent<charger>();
						
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
				if(charge1.charge == -2)
				{
					collision.gameObject.transform.position = gameObject.transform.position + new Vector3(0.5f, -0.6f, 0);
					cjoint.connectedAnchor = new Vector3(0.5f, -0.6f, 0);
				}
				if(charge1.charge == -1)
				{
					collision.gameObject.transform.position = gameObject.transform.position + new Vector3(0f, .750f, 0);
					cjoint.connectedAnchor = new Vector3(0f, .750f, 0);
				}
				
				charge1.updateCharge(charge1.charge + charge2.charge);
				charge2.updateCharge(0f);
				
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
		}
	}
}