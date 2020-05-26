using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JouleDisplayController : MonoBehaviour
{
    public int TotalJoulesDisplaying;
    public GameObject Joule;

    // Start is called before the first frame update
    void Start()
    {
        TotalJoulesDisplaying = 0;   
    }

    public void AddJoules(int JoulesToAdd)
    {
        TotalJoulesDisplaying += JoulesToAdd;
        for (int i = 0; i < JoulesToAdd; i++)
        {
            GameObject GO;
            GO = Instantiate(Joule, gameObject.transform);
            GO.transform.localPosition = new Vector3(Random.Range(-35, 35), Random.Range(-35, 35), 0);
        }
    }

    public void RemoveJoules(int JoulesToRemove)
    {
        if (JoulesToRemove <= TotalJoulesDisplaying)
        {
            TotalJoulesDisplaying -= JoulesToRemove;
            for (int i = 0; i < JoulesToRemove; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
