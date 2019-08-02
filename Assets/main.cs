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
	    //initializes forces
		gameObject.AddComponent<forces>().initialize(G, k);

		// ParticleTest();
    }

    void ParticleTest()
    {
	    // just a debugging function used to test the Particle class and Particle.Spawn() function.
	    Particle sodium1 = new Particle("Sodium1", 1, ICColor.Sodium, new Vector3(5, 0, 0), scale: 2f);
	    Particle chloride1 = new Particle("Chloride1", -1, ICColor.Chlorine, new Vector3(-5, 0, 0), scale: 2f);

	    sodium1.Spawn();
	    chloride1.Spawn();
    }
}