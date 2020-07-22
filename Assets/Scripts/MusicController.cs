using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    private AudioSource Source;

    [Header("Controls")]
    [SerializeField]
    private Button Stop;
    [SerializeField]
    private Button Song1;
    [SerializeField]
    private Button Song2;

    [Header("Songs")]
    [SerializeField]
    private AudioClip FirstClip;
    [SerializeField]
    private AudioClip SecondClip;

    // Start is called before the first frame update
    void Start()
    {
        Source = GetComponent<AudioSource>();
        PlaySong2();
    }
    
    public void StopAudio()
    {
        Source.Stop();
    }

    public void PlaySong1()
    {
        Source.Stop();
        Source.clip = FirstClip;
        Source.Play();
    }

    public void PlaySong2()
    {
        Source.Stop();
        Source.clip = SecondClip;
        Source.Play();
    }
}
