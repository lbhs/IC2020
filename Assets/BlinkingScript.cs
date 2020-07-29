using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingScript : MonoBehaviour
{
    private Text thistext;
    private bool blinking;

    // Start is called before the first frame update
    void Start()
    {
        thistext = GetComponent<Text>();
        blinking = false;

    }

    private void FixedUpdate()
    {
        if (ButtonsDelayedAppearance.frameMarker > 400 && blinking == false)
        {
            StartBlinking();
            blinking = true;
        }

    }

    void StartBlinking()
    {
        StopCoroutine("Blink");
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
        while(true)
        {
            switch(thistext.color.a.ToString())
            {
                case "0":
                    thistext.color = new Color(thistext.color.r, thistext.color.g, thistext.color.b, 1);
                    yield return new WaitForSeconds(1.0f);
                    break;
                case "1":
                    thistext.color = new Color(thistext.color.r, thistext.color.g, thistext.color.b, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
