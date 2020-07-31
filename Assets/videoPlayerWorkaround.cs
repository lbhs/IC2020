using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class videoPlayerWorkaround : MonoBehaviour
{
    public VideoPlayer VP;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        VP.url = "https://lbhs.github.io/Games/Images/4thtry.mp4";
#else
        VP.url = System.IO.Path.Combine(Application.streamingAssetsPath, "4thtry.mp4");
#endif
        VP.Play();
    }
}