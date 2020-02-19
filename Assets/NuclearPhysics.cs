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
            Debug.Log("Collision!");
            /* Improved Method: making collision.gameObject a neutron
            Particle Neutron = new Particle("Neutron " + NeutronCount, 0f, ICColor.Neutron, collision.gameObject.transform.position);
            Neutron.Spawn();    // will be joined to the existing proton (to form isotope of H, 2H)
            NeutronCount++;
            */
        }
    }
}
