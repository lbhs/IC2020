using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionController : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public GameObject[] reactants;
        public GameObject[] products;
    }
    public Data[] Reactions;

    /*
    private void Start()
    {
        Debug.Log(Reactions[0].products);
    }
    */
}
