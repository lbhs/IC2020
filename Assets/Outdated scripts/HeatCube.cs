using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatCube : MonoBehaviour
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
        HeatTransfer ColliderTemp = collider.gameObject.GetComponent<HeatTransfer>();
        print("Mol temp" + ColliderTemp.temp);
        //print(collider.gameObject.GetComponent<HeatTransfer>().temp);
        
        if((ColliderTemp.temp)>BoxTemp && BoxTemp < 3)
        {
            BoxTemp ++;
            print("BoxTemp" + BoxTemp);
            rend.sharedMaterial = material[BoxTemp];
            collider.gameObject.GetComponent<HeatTransfer>().temp --;
        }




    }
}
