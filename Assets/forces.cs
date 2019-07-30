using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forces : MonoBehaviour
{
    private float G;
    private float k;
	//List of all game objects that forces should act on (gravity, electrostatic, collisions, etc.)
    List<GameObject> gameobjects = new List<GameObject>();

	/*
	-Gravitational and coulomb's constants must be initialized on start. i.e.: "gameObject.AddComponent<forces>().initialize(G, k);"
	-Script doesn't run automatically because UI elements must be initialized before forces are applied
	*/
    public void initialize(float gravity, float coulomb)
    {
        G = gravity;
        k = coulomb;
    }

    private GameObject pauseCanvas;
    void Start()
    {
        pauseCanvas = GameObject.Find("Control Canvas");
    }
    //Calculates electrostatic and gravitational forces on all objects in gameobjects list every frame
    void FixedUpdate()
    {
        //Ensures that forces do not get caculated while paused
        if (Time.timeScale != 0)
        {
            //Nested for loops + if statement to calculate force that each object exerts on every other object
            foreach (GameObject a in gameobjects)
            {
                foreach (GameObject b in gameobjects)
                {
                    if (a != b)
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
                        a.GetComponent<Rigidbody>().AddForce(dir * (Fg - Fe) * Time.fixedDeltaTime);
                    }
                }
            }
        }
    }
	
	/*
	-Method to add particles to world via scripting
	-Takes necessary parameters: mass, charge, elastic, position, color, and scale
	-Assumes forces should act on the particle
	-Returns sphere gameobject
	*/
    public GameObject addSphere(float mass, int charge, Vector3 pos, Color color, float scale, float bounciness)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.AddComponent<Rigidbody>();
        sphere.GetComponent<Rigidbody>().mass = mass;
        sphere.GetComponent<Rigidbody>().useGravity = false;
		sphere.GetComponent<Rigidbody>().angularDrag = 0;
        sphere.AddComponent<charger>().charge = charge;
        sphere.transform.position = pos;
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.GetComponent<Renderer>().material.color = color;
        sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
		sphere.GetComponent<Collider>().material.dynamicFriction = 0;
		sphere.GetComponent<Collider>().material.staticFriction = 0;
		sphere.GetComponent<Collider>().material.bounciness = bounciness;
        //Adds the drag object script
        sphere.AddComponent<drag>();
		
        gameobjects.Add(sphere);
        return sphere;
    }
}