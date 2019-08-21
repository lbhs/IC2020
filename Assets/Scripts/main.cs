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
    }
}