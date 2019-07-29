using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    public float G;
    public float k;
    public int n1;
    public int n2;

    
	
    void Start()
    {
        //ui stuff here
		
		//initializes forces
        gameObject.AddComponent<forces>().initialize(G, k);
        gameObject.AddComponent<cubeForces>();

		//example randomly adds several of 2 different kinds of particles
        for(int x = 0; x < n1; x++)
        {
            gameObject.GetComponent<forces>().addSphere(1.0f, -1, new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0), Color.blue, 1);
            gameObject.GetComponent<forces>().addSphere(1.0f, 1, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.red, 1);
        }
       
        //adds n2 amount of bond breaker cubes
        for(int y = 0; y < n2; y++)
        {
            gameObject.GetComponent<cubeForces>().addCube(10.0f, new Vector3(UnityEngine.Random.Range(-5, 5), Random.Range(-5, 5), 0), Color.gray, 1f, new Vector3(UnityEngine.Random.Range(30f, 50f), Random.Range(30f, 50f), 0));
        }
        
    }

    public void AdjustK(float newK)
    {
        k = newK;
    }
}
