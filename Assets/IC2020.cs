//////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                  //
//                              IC2020 (Interactive Chemistry) Library                              //
//                                                                                                  //
//                                          Now with Color!                                         //
//                                                                                                  //
//////////////////////////////////////////////////////////////////////////////////////////////////////

/*
- namespace "IC2020" holds the "Particle" class, "ParticleSpawner" class and "ICColor" static class.
- to use, place 'using IC2020;' (with no quotes) with the other references at the top of a program.
- class "Particle" is a class used to design particles and then spawn them at will.
- class "ParticleSpawner" is a class used to spawn water molecules (and eventually more molecules too).
- static class "ICColor" is a class that holds all of the colors of particles used in IC2020.
- use a gun to launch the particles together
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IC2020
{
    public static class ICColor
    {
        /*
        - static class ICColor is NOT an interface (though its name would imply this).
          "IC" simply stands for "interactive chemistry."
        - "ICColor" uses the same syntax as the "Color" static class.
        - e.g: instead of using Color.red, you would use ICColor.Oxygen.
        - A majority of these colors are not final (especially the electron color).
        */
        public static Color Electron = new Color32(50, 185, 250, 255);
        public static Color Carbon = new Color32(200, 200, 200, 255); 
        public static Color Oxygen = new Color32(240, 0, 0, 255);
        public static Color Hydrogen = new Color32(255, 255, 255, 255);
        public static Color Nitrogen = new Color32(0, 0, 255, 255);
        public static Color Sulfur = new Color32(255, 200, 50, 255);
        public static Color Phosphorus = new Color32(255, 165, 0, 255);
        public static Color Chlorine = new Color32(31, 240, 31, 255);
        public static Color Zinc, Bromine = new Color32(165, 42, 42, 255);
        public static Color Sodium = new Color32(170, 92, 246, 255);
        public static Color Iron = new Color32(255, 165, 0, 255);
        public static Color Magnesium = new Color32(42, 128, 42, 255);
        public static Color Calcium = new Color32(128, 128, 128, 255);
        public static Color Potassium = new Color32(255, 20, 147, 255);
        public static Color Deuterium = new Color32(255, 182, 193, 255);
        public static Color HeliumIsotope = new Color32(238, 221, 130, 255);
        public static Color Neutrino = new Color32(255, 0, 0, 255);
    }

    public class Particle
    {
        /*
        - "Particle" objects are to be treated similarly to "GameObject" objects, but not the same.
        - i.e; both a Particle and a GameObject have a position variable, but the GameObject drawn to the screen,
          while the Particle just has a 'hypothetical position' for where it will be when it is spawned
          as a GameObject.
        */
        
        // Attributes
        private string name; // name of the created GameObject
        private float mass; // mass of the created GameObject's RigidBody
        private bool grav; // if grav = true, the created GameObject's Rigidbody will interact with gravity.
        private float charge; // the charge value of the created GameObject's "charger.cs" script.
        private Vector3 pos = new Vector3(0, 0, 0); // the transform of the created GameObject
        private Color color; // the color of the created GameObject
        private float scale; // the scale of the created GameObject
        private float bounciness; // the bounciness factor of the created GameObject
        private int imgToUse; // the image overlaid on the GameObject (+ or -).

        // Constructor
        public Particle(string name, float charge, Color color, Vector3 pos = new Vector3(), float mass = 1f, float scale = 1f, float bounciness = 0.6f, bool grav = false)
        {
            /*
            - constructor Particle() assigns arguments passed into this function
              to attributes that are local to the object being instanced.
            - parameters "name", "charge", and "color" are required parameters,
              while the other parameters are optional, and have default values.
            - the default values are as follows:
                - Vector3 pos = default(Vector3()) // the position of the particle on spawn.
                - float mass = 1.0f
                - float scale = 1.0f // the scale of the created primitive.
                - float bounciness = 0.6f // the bounciness factor of the created GameObject.
                - bool grav = false 
            */
            // Initializing attributes...
            this.name = name;
            this.mass = mass;
            this.charge = charge;
            this.pos = pos;
            this.color = color;
            this.scale = scale;
            this.bounciness = bounciness;
            this.grav = grav;
            
            if (charge < 0) imgToUse = 1;
            else imgToUse = 0;
        }

        // Particle.Spawn() creates a particle GameObject with all of the predefined attributes above.
        public GameObject Spawn(Vector3 _pos = new Vector3(), bool overridden = false)
        {
            /*
            - Particle.Spawn() creates a primitive sphere GameObject with all of the 
              predefined attributes that were passed into the constructor.
            - It is important to realize that objects of class Particle are NOT GameObjects,
              and never will be, even after using Particle.Spawn(). The Particle object is simply
              a set of instructions for unity to use when creating GameObjects; this is why you can call
              Particle.Spawn() multiple times on the same Particle object.
            - Vector3 _pos is a 3d position vector that can be used to override the position
              that was passed into the constructor. It will only be used if the bool overriden
              evaluates to true. (overidden = false by default).
            - Particle.Spawn() returns GameObject "p" on successful creation,
              and returns null on an unsuccessful creation.
            - the names of all GameObjects created with Particle.Spawn() begin with [P],
              so that they can be recognized by Update() in the forces script
              and added to the gameObjects list to have forces applied to them.
            - all GameObjects created with Particle.Spawn() are given the tag "particle".
            */
            try
            {
                GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                p.name = "[P] " + name;

                // rigidbody initialization
                p.AddComponent<Rigidbody>();
                p.GetComponent<Rigidbody>().mass = mass;
                p.GetComponent<Rigidbody>().useGravity = grav;
                p.GetComponent<Rigidbody>().angularDrag = 0;
                p.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
                p.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
                p.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                // initialization of other components
                p.AddComponent<charger>().charge = charge;
                p.GetComponent<Renderer>().material.color = color;
                p.GetComponent<Collider>().material.dynamicFriction = 0;
                p.GetComponent<Collider>().material.staticFriction = 0;
                p.GetComponent<Collider>().material.bounciness = bounciness;
                p.AddComponent<DragNDrop>();
                p.AddComponent<TimeBody>();

                // if the position is overriden in Particle.Spawn() and overridden == true
                if (overridden) p.transform.position = _pos;
                else p.transform.position = pos;
                p.transform.localScale = new Vector3(scale, scale, scale);

                // overlays the label on top of the GameObject
                GameObject tempLable;
                tempLable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[imgToUse], Vector3.zero, Quaternion.identity);
                tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
                tempLable.GetComponent<ImageFollower>().sphereToFollow = p;

                // p.tag = "Particle"; // raising errors?
                
                Debug.Log("[DEBUG]: Particle " + p.name + " Successfully created.");
                return p;
            }
            catch (Exception e)
            {
                Debug.Log("[DEBUG]: Exception Raised: " + e);
                return null;
            }
        }
    }

    public class MoleculeSpawner
    {
        /*
        - class "MoleculeSpawner" is a class that holds functions for spawning molecules.
        - since MoleculeSpawner is not static, it will need to be instanced in any scripts
          that require it.
        */
        public GameObject AddWater(float xd, float yd)
        {
            /*
            - function AddWater() creates a water molecule.
            - it creates three Particle objects, two representing hydrogen and one representing oxygen.
            - the three particles are tied together using configurable joints.
            - all GameObjects created are given the tag "inWater."
            */
            float hydrox = 0.5f;
            float hydroy = -0.6f;

            float hhx = 0f;
            float hhy = .75f;

            Particle hydrogen1 = new Particle("Hydrogen 1", .1f, ICColor.Hydrogen, new Vector3(xd + hydrox, yd + hydroy, 0), bounciness:0.2f);
            Particle hydrogen2 = new Particle("Hydrogen 2", .1f, ICColor.Hydrogen, new Vector3(xd + hhx, yd + hhy, 0), bounciness:0.2f);
            Particle oxygen = new Particle("Oxygen", -.2f, ICColor.Oxygen, new Vector3(xd, yd, 0), mass: 2, scale: 2f, bounciness:0.2f);

            GameObject _hydrogen1 = hydrogen1.Spawn(new Vector3(xd + hydrox, yd + hydroy, 0));
            GameObject _hydrogen2 = hydrogen2.Spawn(new Vector3(xd + hhx, yd + hhy, 0));
            GameObject _oxygen = oxygen.Spawn(new Vector3(xd, yd, 0));

            /*
            - raising errors?
            _hydrogen1.tag = "inWater";
            _hydrogen2.tag = "inWater";
            _oxygen.tag = "inWater";
            */
            
            ConfigurableJoint cjoint;
            cjoint = _hydrogen1.AddComponent<ConfigurableJoint>();
            cjoint.xMotion = ConfigurableJointMotion.Limited;
            cjoint.yMotion = ConfigurableJointMotion.Limited;
            cjoint.zMotion = ConfigurableJointMotion.Locked;
            cjoint.angularXMotion = ConfigurableJointMotion.Limited;
            cjoint.angularYMotion = ConfigurableJointMotion.Limited;
            cjoint.angularZMotion = ConfigurableJointMotion.Locked;
            cjoint.connectedBody = _oxygen.GetComponent<Rigidbody>();
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
            ccjoint = _hydrogen2.AddComponent<ConfigurableJoint>();
            ccjoint.xMotion = ConfigurableJointMotion.Limited;
            ccjoint.yMotion = ConfigurableJointMotion.Limited;
            ccjoint.zMotion = ConfigurableJointMotion.Locked;

            ccjoint.connectedBody = _oxygen.GetComponent<Rigidbody>();

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
}

// old functions to be deleted / referenced

/*
- Benny's notes from original addSphere() function:
-Method to add particles to world via scripting
-Takes necessary parameters: mass, charge, elastic, position, color, and scale
-Assumes forces should act on the particle
-Returns sphere gameobject
*/
/*
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
    //sets the corrosponding image to the sphere
    GameObject tempLable;
    tempLable = Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[imageToUse], Vector3.zero , Quaternion.identity);
    tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
    tempLable.GetComponent<ImageFollower>().sphereToFollow = sphere;
    gameobjects.Add(sphere);
    return sphere;
}
*/
/*
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
*/