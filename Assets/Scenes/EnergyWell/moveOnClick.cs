using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveOnClick : MonoBehaviour {
    public Rigidbody bodyType;
    Rigidbody m_Rigidbody;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            // gameObject.GetComponent<Rigidbody>(). = !gameObject.GetComponent<Rigidbody>().useGravity;
            m_Rigidbody.isKinematic = false;
        }
    }
}

