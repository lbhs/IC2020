using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
-Code taken from https://unity3d.college/2017/07/03/using-vector3-reflect-to-cheat-ball-bouncing-physics-in-unity/ and modified by me
-To use, add this script as a component to any game object with a rigidbody
-Choose what collision detection according to these guidelines: https://docs.unity3d.com/ScriptReference/Rigidbody-collisionDetectionMode.html
-Collision may not be completely elastic due to nvidia physx optimizations; more testing is required
*/
public class elastic : MonoBehaviour
{
    private Vector3 lastFrameVelocity;
	
    private void Update()
    {
        lastFrameVelocity = GetComponent<Rigidbody>().velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collision.contacts[0].normal);
        GetComponent<Rigidbody>().velocity = direction * speed;
    }
}