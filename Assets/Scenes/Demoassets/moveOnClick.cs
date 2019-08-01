﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveOnClick : MonoBehaviour {
    public RigidbodyType2D bodyType;
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            // gameObject.GetComponent<Rigidbody>(). = !gameObject.GetComponent<Rigidbody>().useGravity;
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
