using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeForces : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
    /*Method to add bond breaker cubes via scripting
         *takes parameters: mass, elastic, position, velocity, color, and scale
         *Collides with other game objects
         * returns cube gameobject
         * */
    public GameObject addCube(float massC, bool elasticC, Vector3 posC, Color colorC, float scaleC, Vector3 velC)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);   //create primitive type of gameobject called cube
        cube.AddComponent<Rigidbody>();                                     //Makes cube a Rigidbody
        cube.GetComponent<Rigidbody>().mass = massC;                        //Defines mass as a property of cube
        cube.GetComponent<Rigidbody>().useGravity = false;                  //Disables Gravity
        cube.GetComponent<Rigidbody>().angularDrag = 0;                     //Disables angular drag
        cube.transform.position = posC;                                     //Defines position as a property of cube
        cube.GetComponent<Renderer>().material.color = colorC;              //Defines color as a property of cube
        cube.transform.localScale = new Vector3(scaleC, scaleC, scaleC);    //Scales the previously defined position
        cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;           //Freezes Z position
                                                                                                                                                                                   //cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;           //Freezes X rotation
                                                                                                                                                                                   //cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;           //Freezes Y rotation

        cube.GetComponent<Rigidbody>().velocity = velC;  //Adds initial velocity as a property of cube
        cube.AddComponent<drag>();

        return cube;             //returns cube as a retrievable entity
        
    }
}
