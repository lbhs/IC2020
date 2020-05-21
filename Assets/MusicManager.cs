using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip song1;
    public AudioClip song2;
    public AudioSource BackgroundMusic;
    public bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = true; 
    }

       

    public void songChange1()
    {
        BackgroundMusic.Stop();
        BackgroundMusic.loop = true;
        BackgroundMusic.clip = song1;
        BackgroundMusic.Play();
    }

    public void songChange2()
    {
        BackgroundMusic.Stop();
        BackgroundMusic.loop = true;
        BackgroundMusic.clip = song2;
        BackgroundMusic.Play();
    }

    public void stopMusic()
    {
        isPlaying = false;
        BackgroundMusic.Stop();
    }


    public void playMusic()
    {
        isPlaying = true;
        BackgroundMusic.Play();
    }

}
