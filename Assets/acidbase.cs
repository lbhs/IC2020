using System;
using UnityEngine;
using IC2020;

public class acidbase : MonoBehaviour
{
	private forces mainObject;
	//public bool bound = false;
	
	//public bool collided;
    // Start is called before the first frame update
    void Start()
    {
        mainObject = GameObject.Find("GameObject").GetComponent<forces>();
		Debug.Log("this farrrr");
    }

    private void OnCollisionEnter(Collision collision)
    {
		GameObject particle1 = gameObject;
		GameObject particle2 = collision.gameObject;
        if(particle2.GetComponent<acidbase>() != null && particle2.GetComponent<Renderer>().material.color == ICColor.Hydrogen)
		{
			Destroy(collision.gameObject.GetComponent<acidbase>()); //DOESNT DESTROY PARTICLE!!!! just destroys the acidbase component - in the future it will set it as able to give away
			charger charge = GetComponent<charger>();
					
			//bind hydrogen to atom
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
			
			//if second water hydrogen then connect at 104.5 degrees, otherwise 
			if(particle1.GetComponent<Renderer>().material.color == ICColor.Oxygen && charge.charge == -1){
				//particle2.transform.position = particle1.transform.position + new Vector3(0.5f, -0.6f, 0);
				cjoint.connectedAnchor = new Vector3(0f, 0f, 0);
				//Destroy(GetComponent<acidbase>()); //not in the future
				//bound = true;
			}
			else
			{
				//collision.gameObject.transform.position = gameObject.transform.position + new Vector3(0f, 0.750f, 0);
				//particle2.transform.position = Vector3.MoveTowards(particle2.transform.position, particle1.transform.position, particle1.transform.localScale.x/2);
				Physics.IgnoreCollision(particle1.GetComponent<SphereCollider>(), particle2.GetComponent<SphereCollider>());
				cjoint.connectedAnchor = new Vector3(0f, 0f, 0);
			}
			
			charge.updateCharge(charge.charge + 1);
			collision.gameObject.GetComponent<charger>().updateCharge(0);
			
			//finish binding hydrogen to atom
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