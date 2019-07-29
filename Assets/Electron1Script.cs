using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron1Script : MonoBehaviour
{
    Rigidbody Electron1;
    GameObject P1;    //this is used to acquire and store information about proton1
    GameObject P2;    //this is used to acquire and store information about proton2
    GameObject E2;      //for Electron2 data
    public Vector3 Electron1Velocity;
    public float charge;
    private float Q1;
    private float Q2;
    private float r;
    private Vector3 dir;
    private float EF;
    private Vector3 force;


    // Start is called before the first frame update
    void Start()
    {
        //initialize Electron1 with Rigidbody properties//
        Electron1 = GetComponent<Rigidbody>();
        
        Electron1.velocity = Electron1Velocity;
        Q1 = charge;
        
        //Initialize ability to collect information about proton positions
        P1 = GameObject.Find("Proton1");      //"Finds" Proton1 and stores in variable P1
        P1.GetComponent<Proton1Script>();     //allows access to Proton1Script, which has position and charge info
   
        P2 = GameObject.Find("Proton2");      //"Finds" Proton2 and stores in variable P2
        P2.GetComponent<Proton2Script>();     //allows access to Proton2Script, which has position and charge info 
        
        E2 = GameObject.Find("Electron2");   //Associates Electron2 info with variable E2
        E2.GetComponent<Electron2Script>();  //allows access to Electron2Script
        
        }

    // FixedUpdate used to calculate Physics operations
    void FixedUpdate()
    {
        //Calculate attraction of Electron1 to Proton1
        //Retrieve value of Proton1 Charge
        Q2 = P1.GetComponent<Proton1Script>().charge;
        
        //calculate distance between Electron1 and Proton1
        r = Vector3.Distance(P1.transform.position, Electron1.transform.position);
        
        //calculate direction of force vector between Electron1 and Proton1 
        dir = Vector3.Normalize(P1.transform.position - Electron1.transform.position);
        
        //calculate magnitude of force between E1 and P1 --Coulomb's Law (K = 10)
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        //print("electron F1");
        //print(force);
        
        Electron1.AddForce(force);
        
        //Calculate attractions between Electron1 and Proton2
        //Retrieve value of Proton2 Charge
        Q2 = P2.GetComponent<Proton2Script>().charge;
        
        //calculate distance between Electron1 and Proton2
        r = Vector3.Distance(P2.transform.position, Electron1.transform.position);
       
        //calculate direction of force vector between Electron1 and Proton2 
        dir = Vector3.Normalize(P2.transform.position - Electron1.transform.position);
        
        //calculate magnitude of force between E1 and P2 --Coulomb's Law (K = 10)
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
                
        Electron1.AddForce(force);

    //Calculate repulsion between  Electron1 and Electron2
        //Retrieve value of Electron2 Charge
        Q2 = E2.GetComponent<Electron2Script>().charge;
        
        //calculate distance between Electron2 and Electron1
        r = Vector3.Distance(E2.transform.position, Electron1.transform.position);
        
        //calculate direction of force vector between Electron2 and Electron1 
        dir = Vector3.Normalize(E2.transform.position - Electron1.transform.position);
        
        //calculate magnitude of force between E2 and E1 --Coulomb's Law (K = 10)
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
                        
        Electron1.AddForce(force);

    }
}
