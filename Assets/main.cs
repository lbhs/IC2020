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

    private void Start()
    {
	    // Initializes forces
		gameObject.AddComponent<forces>().initialize(G, k);

		// ParticleTest();
    }

    private void ParticleTest()
    {
	    // just a debugging function used to test the Particle class and Particle.Spawn() function.
	    Particle electron = new Particle("Electron", -1, ICColor.Electron, new Vector3(-5, 0, 0), scale:1f);
	    Particle nitrogen = new Particle("Nitrogen", -3, ICColor.Nitrogen, new Vector3(5, 0, 0), scale: 2f);
	    // Particle chloride1 = new Particle("Chloride1", -1, ICColor.Chlorine, new Vector3(-5, 0, 0), scale: 2f);

	    electron.Spawn(0f);
	    nitrogen.Spawn(0f);
	    // chloride1.Spawn();
    }
}