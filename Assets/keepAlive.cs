using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepAlive : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<Rigidbody>().velocity.magnitude < 1f) {
			gameObject.GetComponent<Rigidbody>().velocity *= 1.4f;
			Debug.Log("staying alive");
		}
    }
}
