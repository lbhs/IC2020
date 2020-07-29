using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGameOver : MonoBehaviour
{
  public AudioSource MyAudioSource;
  public bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        MyAudioSource = GetComponent<AudioSource>();
        isPlaying = true;
        MyAudioSource.Play();
        MyAudioSource.loop = true; //test
    }

    // Update is called once per frame
    void Update()
    {

    }
}
