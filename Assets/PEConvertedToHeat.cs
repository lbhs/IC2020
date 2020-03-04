using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEConvertedToHeat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject.Find("EnergyText").GetComponent<Text>().text = "PE converted to Heat = " + ((int)GlobalTempMotion.Joules-50).ToString() + " Joules";
    }
}
