using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proton2Script : MonoBehaviour
{
    Rigidbody Proton2;
    GameObject E1;    //for Electron1 position and charge info
    GameObject P1;    //for Proton1 position and charge info
    GameObject E2;    //for Electron2 data
    public Vector3 Proton2Position;
    public Vector3 Proton2Velocity;
    public float charge;
    private float Q1;
    private float Q2;
    private float r;
    private float EF;
    private Vector3 dir;
    private Vector3 force;

    // Start is called before the first frame update
    void Start()
    {
        Proton2 = GetComponent<Rigidbody>();  //allows access to position and velocity vectors for this object P2
        Proton2.velocity = Proton2Velocity;   //initializes Proton2 velocity to be that shown in the Inspector
        Q1 = charge;   //defines Q1 as the charge shown in the inspector
        
        //initialize ability to collect information about Electron1 
        E1 = GameObject.Find("Electron1");      //associates the variable E1 with Electron 1
        E1.GetComponent<Electron1Script>();   //allows this script to access Electron1Script components

        //initialize ability to collect information about Electron2 
        E2 = GameObject.Find("Electron2");      //associates the variable E1 with Electron 1
        E2.GetComponent<Electron2Script>();   //allows this script to access Electron1Script components
        
        //initialize ability to collect information about Proton1 
        P1 = GameObject.Find("Proton1");      //associates the variable P1 with Proton 1
        P1.GetComponent<Proton1Script>();   //allows this script to access Proton1Script components

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Proton2 attraction to Electron1
        //acquire value of Q2 from Electron1Script (Electron1 linked to variable E1)
        Q2 = E1.GetComponent<Electron1Script>().charge;
        
        //calculate distance between P1 and E1--store value as "r"
        r = Vector3.Distance(Proton2.transform.position, E1.transform.position);
                
        //calculate direction vector between P1 and E1
        dir = Vector3.Normalize(E1.transform.position - Proton2.transform.position);
        
        //calculate magnitude of electrostatic force --Coulomb's Law (K = 100)
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        print("P2 attraction to electron 1");
        print(EF);
        Proton2.AddForce(force);

        //Proton2 attraction to Electron2
        //acquire value of Q2 from Electron2Script (Electron2 linked to variable E2)
        Q2 = E2.GetComponent<Electron2Script>().charge;
        
        //calculate distance between P1 and E2--store value as "r"
        r = Vector3.Distance(Proton2.transform.position, E2.transform.position);
                
        //calculate direction vector between P1 and E2
        dir = Vector3.Normalize(E2.transform.position - Proton2.transform.position);
        
        //calculate magnitude of electrostatic force --Coulomb's Law K = 100
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        print("P2 attraction to electron 2");
        print(EF);
        Proton2.AddForce(force);


        //Calculate Proton-Proton Repulsion
        //acquire value of Q2 from Proton1Script (Proton1 linked to variable P1)
        Q2 = P1.GetComponent<Proton1Script>().charge;
       
        //calculate distance between P1 and P2--store value as "r"
        r = Vector3.Distance(Proton2.transform.position, P1.transform.position);
        
        //calculate direction vector between P1 and P2
        dir = Vector3.Normalize(P1.transform.position - Proton2.transform.position);
        
        //calculate magnitude of electrostatic force --Coulomb's Law K = 100
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        
        Proton2.AddForce(force);
    }
}
