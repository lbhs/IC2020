using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JouleHolderScript : MonoBehaviour
{
    public GameObject JUIPrefab;
    

    public void JSpawn()
    {
        GameObject JouleInCorral = Instantiate(JUIPrefab);
        JouleInCorral.transform.parent = gameObject.transform;
        JouleInCorral.transform.localPosition = new Vector3(Random.Range(-28, 28), Random.Range(-28, 28), 0);
        
    }
    //call with  GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().JSpawn();
}


