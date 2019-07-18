/*
- TODO:
- Document this.
- Re-make the sphere naming convention.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameObject AddSphere(float mass, int charge, bool elastic, Vector3 pos, Color color, float scale)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.AddComponent<Rigidbody>();
        sphere.GetComponent<Rigidbody>().mass = mass;
        sphere.GetComponent<Rigidbody>().useGravity = false;
        sphere.GetComponent<Rigidbody>().angularDrag = 0;
        if (elastic)
        {
            sphere.AddComponent<Elastic>();
        }
        sphere.AddComponent<Charger>().charge = charge;
        sphere.transform.position = pos;
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.GetComponent<Renderer>().material.color = color;
        sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

        //Adds the drag object script
        sphere.AddComponent<Drag>();

        GetComponent<Forces>().gameobjects.Add(sphere);
        return sphere;
    }
}
