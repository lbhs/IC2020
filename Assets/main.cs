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
    public bool useFlick;
	
	void Start()
	{
		gameObject.AddComponent<forces>().initialize(G, k);
        Particle oxide = new Particle("Oxide", -2f, ICColor.Oxygen, new Vector3(0, 0, 0), mass: 2, scale: 2f, bounciness:0.2f);
		Particle hydrogen1 = new Particle("Hydrogen 1", 1f, ICColor.Hydrogen, new Vector3(4, 4, 0), bounciness:0.2f);
		Particle hydrogen2 = new Particle("Hydrogen 1", 1f, ICColor.Hydrogen, new Vector3(-6, 5, 0), bounciness:0.2f);
        GameObject _oxygen = oxide.Spawn(new Vector3(0, 0, 0));
		GameObject _hydrogen1 = hydrogen1.Spawn(new Vector3(4, 4, 0));
		GameObject _hydrogen2 = hydrogen2.Spawn(new Vector3(-3, 2, 0));
		_oxygen.AddComponent<LabelAssigner>().Add(true, "O");
		_hydrogen1.AddComponent<LabelAssigner>().Add(true, "H");
		_hydrogen2.AddComponent<LabelAssigner>().Add(true, "H");
		
		MoleculeSpawner m = new MoleculeSpawner();
		m.AddWater(-6, -6);
	}
}