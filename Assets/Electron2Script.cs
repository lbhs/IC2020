using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron2Script : MonoBehaviour
{
    Rigidbody Electron2;
    GameObject P1;    //this is used to acquire and store information about proton1
    GameObject P2;    //this is used to acquire and store information about proton2
    GameObject E1;   //this is used to save info about electron1
    public Vector3 Electron2Velocity;
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
        //initialize Electron2 with Rigidbody properties//
        Electron2 = GetComponent<Rigidbody>();
        //set Rigidbody parameters to those put into the inspector//
        Electron2.velocity = Electron2Velocity;
        Q1 = charge;
        
        //Initialize ability to collect information about proton positions
        P1 = GameObject.Find("Proton1");      //"Finds" Proton1 and stores in variable P1
        P1.GetComponent<Proton1Script>();     //allows access to Proton1Script, which has position and charge info
   
        P2 = GameObject.Find("Proton2");      //"Finds" Proton2 and stores in variable P1
        P2.GetComponent<Proton2Script>();     //allows access to Proton2Script, which has position and charge info 

        E1 = GameObject.Find("Electron1");    //finds Electron1--data stored in E1
        E1.GetComponent<Electron1Script>();
        }

    // FixedUpdate used to calculate Physics operations
    void FixedUpdate()
    {
        //Retrieve value of Proton1 Charge
        Q2 = P1.GetComponent<Proton1Script>().charge;
        
        //calculate distance between Electron2 and Proton1
        r = Vector3.Distance(P1.transform.position, Electron2.transform.position);
        
        //calculate direction of force vector between Electron2 and Proton1 
        dir = Vector3.Normalize(P1.transform.position - Electron2.transform.position);
        
        //calculate magnitude of force between E2 and P1 --Coulomb's Law (K = 10)
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        print("E2 attractions to P1");  //allows user to monitor forces in Console output
        print(EF);
        
        Electron2.AddForce(force);
        
        //Retrieve value of Proton2 Charge
        Q2 = P2.GetComponent<Proton2Script>().charge;
        //print(Q2);
        //calculate distance between Electron2 and Proton2
        r = Vector3.Distance(P2.transform.position, Electron2.transform.position);
        //print(r);
        //calculate direction of force vector between Electron2 and Proton2 
        dir = Vector3.Normalize(P2.transform.position - Electron2.transform.position);
        //print(dir);
        //calculate magnitude of force between E2 and P2 --Coulomb's Law (K = 100)
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        print("E2 attraction to P2");
        print(EF);
        
        Electron2.AddForce(force);

        //Retrieve value of Electron1 Charge
        Q2 = E1.GetComponent<Electron1Script>().charge;
        
        //calculate distance between Electron2 and Electron1
        r = Vector3.Distance(E1.transform.position, Electron2.transform.position);
        
        //calculate direction of force vector between Electron2 and Electron1
        dir = Vector3.Normalize(E1.transform.position - Electron2.transform.position);
        
        //calculate magnitude of force between E1 and E2 --Coulomb's Law (K = 100)
        EF = 100*Q1*Q2/(r*r);
        
        force = -EF*dir;
        print("electron repulsion force");
        print(EF);
        
        Electron2.AddForce(force);  

    }
}
