using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class forces : MonoBehaviour
{
    private float G;
    private float k;
    private GameObject pauseCanvas;
    private Scene scene;

	//List of all game objects that forces should act on (gravity, electrostatic, collisions, etc.)
    [FormerlySerializedAs("gameobjects")] public List<GameObject> gameObjects = new List<GameObject>();
    private List<GameObject> rootObjects = new List<GameObject>();
    
	/*
	-Gravitational and coulomb's constants must be initialized on start. i.e.: "gameObject.AddComponent<forces>().initialize(G, k);"
	-Script doesn't run automatically because UI elements must be initialized before forces are applied
	*/
    public void initialize(float gravity, float coulomb)
    {
        G = gravity;
        k = coulomb;
    }
    
    void Start()
    {
        pauseCanvas = GameObject.Find("Control Canvas");
        scene = SceneManager.GetActiveScene();
    }
    
    private void Update()
    {
        /*
        - Update() finds all of the objects in the scene and adds the ones which start with [P]
          to the list gameObjects.
        */
        scene.GetRootGameObjects(rootGameObjects: rootObjects);
        foreach (GameObject o in rootObjects)
        {
            string objIdentifier = o.name[0].ToString() + o.name[1].ToString() + o.name[2].ToString();

            if (!gameObjects.Contains(o) && objIdentifier == "[P]")
            {
                gameObjects.Add(o);
                Debug.Log("[DEBUG]: Object " + o.name + " successfully added to list gameObjects.");
            }
        }
    }
    
    //Calculates electrostatic and gravitational forces on all objects in gameObjects list every frame
    private void FixedUpdate()
    {
        //Ensures that forces do not get calculated while paused
        if (Time.timeScale != 0)
        {
            //Nested for loops + if statement to calculate force that each object exerts on every other object
            foreach (GameObject a in gameObjects)
            {
                foreach (GameObject b in gameObjects)
                {
                    if (a != b && a.HasComponent<Rigidbody>() && b.HasComponent<Rigidbody>())
                    {
                        //all variable retrieval necessary for force math					
                        float m1 = a.GetComponent<Rigidbody>().mass;
                        float m2 = b.GetComponent<Rigidbody>().mass;
                        float q1 = a.GetComponent<charger>().charge;
                        float q2 = b.GetComponent<charger>().charge;
                        Vector3 dir = Vector3.Normalize(b.transform.position - a.transform.position);
                        float r = Vector3.Distance(a.transform.position, b.transform.position);

                        //individually calculates force of gravity and electrostatics
                        float Fg = (m1 * m2 * G) / Mathf.Pow(r, 2);
                        float Fe = (k * q1 * q2) / Mathf.Pow(r, 2);

                        //applies force vector
						//must use time.fixeddeltatime here to keep constant framerate with different timescales
                        a.GetComponent<Rigidbody>().AddForce(dir * (Fg - Fe) * Time.fixedDeltaTime);
                    }
                }
            }
        }
    }
}
