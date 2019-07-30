using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempatureController : MonoBehaviour
{
    public void IncreaseTempOfAll()
    {
        foreach (GameObject spheres in GameObject.Find("GameObject").GetComponent<forces>().gameobjects)
        {
            spheres.GetComponent<Rigidbody>().AddForce(new Vector3(spheres.GetComponent<Rigidbody>().velocity.x * 2, spheres.GetComponent<Rigidbody>().velocity.y * 2, 0), ForceMode.Impulse);
        }
    }
    public void DecreaseTempOfAll()
    {
        foreach (GameObject spheres in GameObject.Find("GameObject").GetComponent<forces>().gameobjects)
        {
            spheres.GetComponent<Rigidbody>().AddForce(new Vector3(-spheres.GetComponent<Rigidbody>().velocity.x * .5f, -spheres.GetComponent<Rigidbody>().velocity.y / 2, 0), ForceMode.Impulse);
            //Debug.Log(spheres.GetComponent<Rigidbody>().velocity);
        }
    }
}
