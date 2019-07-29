using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSlector : MonoBehaviour
{
    public GameObject Pannel;
    public int n;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenEmptyScene()
    {
        Pannel.SetActive(false);
    }
}
        //example randomly adds several of 2 different kinds of particles
        /*for (int x = 0; x < n; x++)
        {
            gameObject.GetComponent<forces>().addSphere(1.0f, 1, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.blue, 1);
            gameObject.GetComponent<forces>().addSphere(2.0f, 2, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.red, 2);
        }*/
