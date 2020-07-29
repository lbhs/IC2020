using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public bool Tutorial;
    public GameObject DiceActiveOrNot;
    public GameObject Turn3Prefab;

    // Start is called before the first frame update
    void Start()
    {
        //Tutorial = true;
        if(Tutorial == true)
        {
            GameObject.Find("DiceButton").SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
