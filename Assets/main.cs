using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    public float G;
    public float k;
  //public int n;
	
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
    }

    public void changeTemperature(float scale)
    {

    }
    }