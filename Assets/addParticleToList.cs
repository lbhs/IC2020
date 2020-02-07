using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addParticleToList : MonoBehaviour
{
    void OnEnable()
    {
        GameObject.Find("GameObject").GetComponent<forces>().gameObjects.Add(gameObject);
    }
}
