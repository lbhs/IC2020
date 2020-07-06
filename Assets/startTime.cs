using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class startTime : MonoBehaviour
{
    public GameObject button;

    // Start is called before the first frame update
    void Start()
    {
        button = GameObject.Find("Button");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void begin()
    {
        Time.timeScale = 0.25f;
        button.SetActive(false);
    }
}
