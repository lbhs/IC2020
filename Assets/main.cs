using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IC2020;

public class main : MonoBehaviour
{
    public float G;
    public float k;
    public List<GameObject> particles = new List<GameObject>();
    public int numWater;

    void Start()
    {
	    //ui stuff here
		
		//initializes forces
		gameObject.AddComponent<forces>().initialize(G, k);

		//example randomly adds several of 2 different kinds of particles
        //------ moved this behivor to ModelSlector-------
        /*for(int x = 0; x < n; x++)
        {
            gameObject.GetComponent<forces>().addSphere(1.0f, -1, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.blue, 1, 0.6f, 1);
            gameObject.GetComponent<forces>().addSphere(2.0f, 2, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.red, 2, 0.6f, 0);
        }
		*/
        // ParticleTest();
    }

    void ParticleTest()
    {
	    Particle sodium1 = new Particle("Sodium1", 1, ICColor.Sodium, new Vector3(5, 0, 0), scale: 2f);
	    Particle chloride1 = new Particle("Chloride1", -1, ICColor.Chlorine, new Vector3(-5, 0, 0), scale: 2f);

	    sodium1.Spawn();
	    chloride1.Spawn();
    }
    
}