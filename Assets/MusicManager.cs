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

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            songChange1();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            songChange2();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isPlaying == true)
            {
                stopMusic();
            }
            else if (isPlaying == false)
            {
                playMusic();
            }
        }*/
    }

    public void songChange1()
    {
        BackgroundMusic.Stop();
        BackgroundMusic.loop = true;
        BackgroundMusic.PlayOneShot(song2);
    }

    public void songChange2()
    {
        BackgroundMusic.Stop();
        BackgroundMusic.loop = true; //test
        BackgroundMusic.PlayOneShot(song1);
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
