using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textDisabler : MonoBehaviour
{

	// Update is called once per frame
	void FixedUpdate()
	{
		foreach (Collider obj in Physics.OverlapSphere(new Vector3(2, 0, 0), 30))
		{
			if (obj.gameObject.GetComponent<O2GasScript>() != null)
			{
				if (obj.gameObject.transform.position.x < -10f)
				{
					obj.gameObject.GetComponent<LabelAssigner>().tempLable.GetComponent<Text>().text = "";
				}
				else
				{
					obj.gameObject.GetComponent<LabelAssigner>().tempLable.GetComponent<Text>().text = "O2";
				}
			} else if(obj.gameObject.GetComponent<GasMoleculeMotion>() != null) {
				if (obj.gameObject.transform.position.x < -10f)
				{
					obj.gameObject.GetComponent<LabelAssigner>().tempLable.GetComponent<Text>().text = "";
				}
				else
				{
					obj.gameObject.GetComponent<LabelAssigner>().tempLable.GetComponent<Text>().text = "N2";
				}
			}
		}
	}
}