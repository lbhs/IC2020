using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    private AudioSource Source;
    private Slider VolumeControl;
    private Text VolumeText;

    public AudioClip FirstClip;
    public AudioClip SecondClip;

    // Start is called before the first frame update
    void Start()
    {
        Source = GetComponent<AudioSource>();
        VolumeControl = transform.GetChild(0).GetComponent<Slider>();
        VolumeText = transform.GetChild(1).GetComponent<Text>();

        Source.volume = 0;
        VolumeControl.value = 0;
        VolumeText.text = "Volume: 0%";
    }

    public void SliderValueChanged()
    {
        Source.volume = VolumeControl.value;
        VolumeText.text = string.Format("Volume: {0}%", Mathf.Round(VolumeControl.value * 100));
    }
}
