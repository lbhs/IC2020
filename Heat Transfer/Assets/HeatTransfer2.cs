using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatTransfer2 : MonoBehaviour
{
    private Rigidbody cube;
    public int BoxTemp;
    public Material[] material;
    Renderer rend;
    private GameObject ColliderTemp;
    

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = material[BoxTemp];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collider)
    {
        print(collider.gameObject.tag);
        IndividMolVelocity ColliderTemp = collider.gameObject.GetComponent<IndividMolVelocity>();
        print("Molc temp" + ColliderTemp.temp);
        print("Box temp" + BoxTemp);
        //print("Molc temp" + collider.gameObject.GetComponent<IndividMolVelocity>().temp);
        
        if((ColliderTemp.temp)>BoxTemp && BoxTemp < 4)
        {
            BoxTemp ++;
            print("BoxTemp increased to" + BoxTemp);
            rend.sharedMaterial = material[BoxTemp];
            collider.gameObject.GetComponent<IndividMolVelocity>().temp --;
            print("Molc temp decreased to" + collider.gameObject.GetComponent<IndividMolVelocity>().temp);
            
        }
        else if((ColliderTemp.temp) < BoxTemp && BoxTemp > 1)
        {
            BoxTemp --;
            print("BoxTemp decreased to" + BoxTemp);
            rend.sharedMaterial = material[BoxTemp];
            collider.gameObject.GetComponent<IndividMolVelocity>().temp ++;

        }


    }
}
