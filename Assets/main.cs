using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    public float G;
    public float k;
    public int n;
	
    void Start()
    {
        //ui stuff here
		
		//initializes forces
        gameObject.AddComponent<forces>().initialize(G, k);

		//example randomly adds several of 2 different kinds of particles
        for(int x = 0; x < 2; x++)
        {
            //gameObject.GetComponent<forces>().addSphere(1.0f, 1f, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.blue, 1);
            //gameObject.GetComponent<forces>().addSphere(2.0f, -1f, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.red, 2);
			gameObject.GetComponent<forces>().addWater(UnityEngine.Random.Range(-4f,4f),UnityEngine.Random.Range(-4f,4f));
            
        }
        //gameObject.GetComponent<forces>().addWater(4f,4f);gameObject.GetComponent<forces>().addSphere(2.0f, -1f, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.red, 2);
		gameObject.GetComponent<forces>().addSphere(2.0f, 150f, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.red, 2);
    }
}
