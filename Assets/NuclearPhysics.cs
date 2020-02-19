using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IC2020;

/*
 * This new component will be added to all protons in the Nucleosynthesis simulation
 * This component gives nuclear physics abilities to the particles in question
 * Ideas:
 *  Each of the particles in the 2H isotope (1 P, 1 P->N) of H will have the name '[P] 2Hydrogen' instead of '[P] Hydrogen'
 *  When two protons collide, collision.gameObject can become a neutron to form the 2H isotope (no new neutron instantiated)
*/

public class NuclearPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) { 
        // int NeutronCount = 0;
        // Proton-Proton collision
        if (collision.gameObject.name.Split(' ')[1] == "Hydrogen" && name.Split(' ')[1] == "Hydrogen")
        {
            gameObject.name = "[P] 2Hydrogen";    // Identify particles as belonging to an isotope
            collision.gameObject.name = "[P] 2Hydrogen";
            collision.gameObject.GetComponent<charger>().charge = 0f;
            collision.gameObject.GetComponent<Renderer>().material.color = ICColor.Neutron;

            /* Attempt: change image displayed on new neutron to reflect its neutral charge
            GameObject tempLable;
            tempLable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[2], Vector3.zero, Quaternion.identity);
            tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
            tempLable.GetComponent<ImageFollower>().sphereToFollow = collision.gameObject;
            collision.gameObject.GetComponent<Renderer>().material.color = ICColor.Neutron;
            */

            // Join proton and neutron
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();

            // Release a beta particle and neutrino
            Particle beta = new Particle("[P] Beta", 1.0f, ICColor.Electron);
            GameObject betaObject = beta.Spawn();

            Particle neutrino = new Particle("[P] Neutrino", 0.1f, ICColor.Electron);
            GameObject neutrinoObject = neutrino.Spawn();
        }
    }
}
