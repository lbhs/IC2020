//////////////////////////////////////////////////////////
//                                                      //
//            IC2020 (Interactive Chemistry)            //
//                                                      //
//                       Credits                        //
//                  [Credits Go Here]                   //
//                                                      //
//////////////////////////////////////////////////////////

/*
-Class Main is derived from Unity's built-in MonoBehaviour Class.
-Main.cs is a C# script that calls on other scripts to initialize graphics and forces.
-It should only ever be attached to the GameObject "ControllerRandy."
-GameObject "ControllerRandy" is an empty object that holds any scripts necessary for the initialization / running of Force and Physics calculations,
-as well as the initialization of Graphics and UI.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Debug variables for the initialization of Forces.
    public float G;
    public float k;

    // Variables for use with SpawnSpheresOnStart().
	public bool spawnSpheresOnStart;
    public int numSpherePairs;

    void Start()
    {
        // UI / Graphics Initialization
        // [insert any UI / Graphics initialization here]
		
		// Force Initialization
        gameObject.AddComponent<Forces>().initialize(G, k);
        SpawnSpheresOnStart();
    }

    // Debug function to randomly spawn spawn spheres on start.
	void SpawnSpheresOnStart()
	{
        if (spawnSpheresOnStart)
        {
			for (int x = 0; x < numSpherePairs; x++)
			{
				gameObject.GetComponent<SphereSpawner>().AddSphere(1.0f, 1, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.blue, 1);
				gameObject.GetComponent<SphereSpawner>().AddSphere(2.0f, -2, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.red, 2);
			}
		}
	}
}
