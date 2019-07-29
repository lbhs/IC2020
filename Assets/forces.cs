using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forces : MonoBehaviour
{
    private float G;
    private float k;
	//List of all game objects that forces should act on (gravity, electrostatic, collisions, etc.)
    public List<GameObject> gameobjects = new List<GameObject>();

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
    public GameObject addSphere(float mass, int charge, bool elastic, Vector3 pos, Color color, float scale, int imageToUse)
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
        sphere.AddComponent<DragNDrop>();

        //sets the corrosponding image to the sphere
        GameObject tempLable;
        tempLable = Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[imageToUse], Vector3.zero , Quaternion.identity);
        tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
        tempLable.GetComponent<ImageFollower>().sphereToFollow = sphere;

        gameobjects.Add(sphere);
        return sphere;
    }
}