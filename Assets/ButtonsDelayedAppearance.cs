using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsDelayedAppearance : MonoBehaviour
{
    public static int frameMarker;
    public GameObject TutorialButton;
    public GameObject StandardGameButton;
    public GameObject CreditsButton;
    public GameObject TitleButton;
    
    // Start is called before the first frame update
    void Start()
    {
        frameMarker = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        frameMarker++;
        if(frameMarker > 50)
        {
            TitleButton.SetActive(true);
        }

        if (frameMarker > 200)
        {
            TutorialButton.SetActive(true);        
            StandardGameButton.SetActive(true);
            CreditsButton.SetActive(true);
        }
    }
}
