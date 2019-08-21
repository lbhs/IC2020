using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forces : MonoBehaviour
{
    private float G;
    private float k;
	public bool recording = true;
	public bool pressed = false;
	//private bool rewind = false;
	//List of all game objects that forces should act on (gravity, electrostatic, collisions, etc.)
    public List<GameObject> gameobjects = new List<GameObject>();

	/*
	public bool isRewinding
	{
		get
		{
			return rewind;
		}
		set
		{
			rewind = value;
			if (rewind)
			{
				foreach(GameObject gameObject in gameobjects)
				{
					gameObject.GetComponent<TimeBody>().StartRewind();
				}
			}
			else
			{
				foreach(GameObject gameObject in gameobjects)
				{
					gameObject.GetComponent<TimeBody>().StopRewind();
				}
			}
		}
	}
	*/

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			pressed = !pressed;
			if(pressed)
			{
				startRewind();
			}
			else
			{
				stopRewind();
			}
		}
	}
	/*
	-Gravitational and coulomb's constants must be initialized on start. i.e.: "gameObject.AddComponent<forces>().initialize(G, k);"
	-Script doesn't run automatically because UI elements must be initialized before forces are applied
	*/
    public void initialize(float gravity, float coulomb)
    {
        G = gravity;
        k = coulomb;
    }

	public void stopRewind()
	{
		foreach(GameObject gameObject in gameobjects)
		{
			gameObject.GetComponent<TimeBody>().isRewinding = false;
            GetComponent<TimeBody>().isRewinding = false;
        }
	}
	
	public void startRewind()
	{
		foreach(GameObject gameObject in gameobjects)
		{
			gameObject.GetComponent<TimeBody>().StartRewind();
            GetComponent<TimeBody>().StartRewind();
        }
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
        if (Time.timeScale != 0 && recording)
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
						//must use time.fixeddeltatime here to keep constant framerate with different timescales
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
    public GameObject addSphere(float mass, float charge, Vector3 pos, Color color, float scale, float bounciness, int imageToUse)
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
        sphere.AddComponent<DragNDrop>();
		sphere.AddComponent<TimeBody>();
        //sets the corrosponding image to the sphere
        GameObject tempLable;
        tempLable = Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[imageToUse], Vector3.zero , Quaternion.identity);
        tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
        tempLable.GetComponent<ImageFollower>().sphereToFollow = sphere;
        gameobjects.Add(sphere);
        return sphere;
    }

    public GameObject addWater(float xd, float yd)
    {
        float hydrox = 0.5f;
        float hydroy = -0.6f;

        float hhx = 0f;
        float hhy = .75f;

        GameObject hydrogen = gameObject.GetComponent<forces>().addSphere(1.0f, .1f, new Vector3(xd + hydrox, yd + hydroy, 0), Color.yellow, 1, 0.2f , 2);
        GameObject oxygen = gameObject.GetComponent<forces>().addSphere(2.0f, -.2f, new Vector3(xd, yd, 0), Color.green, 2, 0.2f, 2);
        GameObject hh = gameObject.GetComponent<forces>().addSphere(1.0f, .1f, new Vector3(xd + hhx, yd + hhy, 0), Color.yellow, 1, 0.2f, 2);

        ConfigurableJoint cjoint;
        cjoint = hydrogen.AddComponent<ConfigurableJoint>();
        cjoint.xMotion = ConfigurableJointMotion.Limited;
        cjoint.yMotion = ConfigurableJointMotion.Limited;
        cjoint.zMotion = ConfigurableJointMotion.Locked;
        cjoint.angularXMotion = ConfigurableJointMotion.Limited;
        cjoint.angularYMotion = ConfigurableJointMotion.Limited;
        cjoint.angularZMotion = ConfigurableJointMotion.Locked;
        cjoint.connectedBody = oxygen.GetComponent<Rigidbody>();
        cjoint.anchor = Vector3.down;
        cjoint.angularXMotion = ConfigurableJointMotion.Limited;
        cjoint.angularYMotion = ConfigurableJointMotion.Limited;
        cjoint.angularZMotion = ConfigurableJointMotion.Locked;

        cjoint.autoConfigureConnectedAnchor = false;


        cjoint.connectedAnchor = new Vector3(hydrox, hydroy, 0);

        var limit = new SoftJointLimit();
        limit.limit = 0.1f;
        //limit.SoftJointLimitSpring = 40.0f;
        cjoint.linearLimit = limit;

        limit.limit = 10.0f;
        cjoint.angularYLimit = limit;
        cjoint.angularZLimit = limit;
        cjoint.lowAngularXLimit = limit;
        cjoint.highAngularXLimit = limit;



        ConfigurableJoint ccjoint;
        ccjoint = hh.AddComponent<ConfigurableJoint>();
        ccjoint.xMotion = ConfigurableJointMotion.Limited;
        ccjoint.yMotion = ConfigurableJointMotion.Limited;
        ccjoint.zMotion = ConfigurableJointMotion.Locked;

        ccjoint.connectedBody = oxygen.GetComponent<Rigidbody>();

        ccjoint.angularXMotion = ConfigurableJointMotion.Limited;
        ccjoint.angularYMotion = ConfigurableJointMotion.Limited;
        ccjoint.angularZMotion = ConfigurableJointMotion.Locked;

        ccjoint.autoConfigureConnectedAnchor = false;
        ccjoint.connectedAnchor = new Vector3(hhx, hhy, 0f);

        var llimit = new SoftJointLimit();
        llimit.limit = 0.1f;
        //limit.SoftJointLimitSpring = 40.0f;
        cjoint.linearLimit = llimit;

        llimit.limit = 10.0f;
        cjoint.angularYLimit = llimit;
        cjoint.angularZLimit = llimit;
        cjoint.lowAngularXLimit = llimit;
        cjoint.highAngularXLimit = llimit;


        return null;
    }
}
