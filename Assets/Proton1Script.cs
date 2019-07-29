using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proton1Script : MonoBehaviour
{
    Rigidbody Proton1;
    GameObject E1;    //for Electron1 position and charge info
    GameObject P2;      //for Proton2 position and charge
    GameObject E2;      //for Electron2 data
    public Vector3 Proton1Position;
    public Vector3 Proton1Velocity;
    public float charge;
    private float Q1;    //represents the charge of this proton
    private float Q2;    //this value changes to represent charge of the other object
    private float r;
    private float EF;
    private Vector3 dir;
    private Vector3 force;

    // Start is called before the first frame update
    void Start()
    {
        Proton1 = GetComponent<Rigidbody>();  //allows this script to access to position and velocity vectors
        Proton1.velocity = Proton1Velocity;   //initializes Proton1 velocity to be that shown in the Inspector
        Q1 = charge;   //defines Q1 as the charge shown in the inspector
        
        //initialize ability to collect information about Electron1 
        E1 = GameObject.Find("Electron1");      //associates the variable E1 with Electron 1
        E1.GetComponent<Electron1Script>();   //allows this script to access Electron1Script components

        //initialize ability to collect information about Electron2 
        E2 = GameObject.Find("Electron2");      //associates the variable E1 with Electron 1
        E2.GetComponent<Electron2Script>();   //allows this script to access Electron1Script components

        //initialize ability to collect information about Proton2 
        P2 = GameObject.Find("Proton2");      //associates the variable P2 with Proton 2
        P2.GetComponent<Proton2Script>();   //allows this script to access Proton2Script components
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Calculating attraction of Proton1 to Electron1
        //acquire value of Q2 from Electron1Script (Electron1 linked to variable E1)
        Q2 = E1.GetComponent<Electron1Script>().charge;
        
        //calculate distance between P1 and E1--store value as "r"
        r = Vector3.Distance(Proton1.transform.position, E1.transform.position);
        
        //calculate direction vector between P1 and E1
        dir = Vector3.Normalize(E1.transform.position - Proton1.transform.position);
        
        //calculate magnitude of electrostatic force --Coulomb's Law K = 100
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        Proton1.AddForce(force);    //applies attractive force between P1 and E1


        //Calculating attraction of Proton1 to Electron2
        //acquire value of Q2 from Electron2Script (Electron2 linked to variable E2)
        Q2 = E2.GetComponent<Electron2Script>().charge;
        
        //calculate distance between P1 and E2--store value as "r"
        r = Vector3.Distance(Proton1.transform.position, E2.transform.position);
        
        //calculate direction vector between P1 and E2
        dir = Vector3.Normalize(E2.transform.position - Proton1.transform.position);
        
        //calculate magnitude of electrostatic force --Coulomb's Law K = 10
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        Proton1.AddForce(force);    //attractive force between P1 and E2 is applied to P1 


    //Calculate proton-proton repulsion
    //acquire value of Q2 from Proton2Script (Proton2 is linked to variable P2)
        Q2 = P2.GetComponent<Proton2Script>().charge;
               
        //calculate distance between P1 and P2--store value as "r"
        r = Vector3.Distance(P2.transform.position, Proton1.transform.position);
                
        //calculate direction vector between P1 and P2
        dir = Vector3.Normalize(P2.transform.position - Proton1.transform.position);
        
        //calculate magnitude of electrostatic force --Coulomb's Law (K = 100)
        EF = 100*Q1*Q2/(r*r);
        
        print("repulsive force between protons");
        print(EF);
        
        force = -EF*dir;
        
        Proton1.AddForce(force);
    }
}
