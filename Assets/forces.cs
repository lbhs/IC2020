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
        pauseCanvas = GameObject.Find("Pause Canvas");
    }
    //Calculates electrostatic and gravitational forces on all objects in gameobjects list every frame
    void Update()
    {
        //Ensures that forces do not get caculated while paused
        if (pauseCanvas.GetComponent<pauseScript>().isPaused == false)
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
    public GameObject addSphere(float mass, int charge, bool elastic, Vector3 pos, Color color, float scale)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.AddComponent<Rigidbody>();
        sphere.GetComponent<Rigidbody>().mass = mass;
        sphere.GetComponent<Rigidbody>().useGravity = false;
		sphere.GetComponent<Rigidbody>().angularDrag = 0;
        if (elastic)
        {
            sphere.AddComponent<elastic>();
        }
        sphere.AddComponent<charger>().charge = charge;
        sphere.transform.position = pos;
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.GetComponent<Renderer>().material.color = color;
        sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

        //Adds the drag object script
        sphere.AddComponent<drag>();

        gameobjects.Add(sphere);
        return sphere;
    }

    /*Method to add bond breaker cubes via scripting
     *takes parameters: mass, elastic, position, velocity, color, and scale
     *Collides with other game objects
     * returns cube gameobject
     * 
    public GameObject addCube(float massC, bool elasticC, Vector3 posC, Color colorC, float scaleC, Vector3 velC)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);   //create primitive type of gameobject called cube
        cube.AddComponent<Rigidbody>();                                     //Makes cube a Rigidbody
        cube.GetComponent<Rigidbody>().mass = massC;                        //Defines mass as a property of cube
        cube.GetComponent<Rigidbody>().useGravity = false;                  //Disables Gravity
        cube.GetComponent<Rigidbody>().angularDrag = 0;                     //Disables angular drag
        if (elasticC)                                                       //Defines elasticity as a property of cube
        {
            cube.AddComponent<elastic>(); 
        }
        cube.transform.position = posC;                                     //Defines position as a property of cube
        cube.GetComponent<Renderer>().material.color = colorC;              //Defines color as a property of cube
        cube.transform.localScale = new Vector3(scaleC, scaleC, scaleC);    //Scales the previously defined position
        cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;           //Freezes Z position
        //cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;           //Freezes X rotation
        //cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;           //Freezes Y rotation
     
        cube.GetComponent<Rigidbody>().velocity = velC;  //Adds initial velocity as a property of cube
        cube.AddComponent<drag>();
        
        gameobjects.Add(cube);
        return cube;



    }
    

    */
}