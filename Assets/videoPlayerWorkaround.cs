﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class videoPlayerWorkaround : MonoBehaviour
{
    public VideoPlayer VP;
    // Start is called before the first frame update
    void Start()
    {
        VP.url = System.IO.Path.Combine(Application.streamingAssetsPath, "4thtry.mp4");
        VP.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}