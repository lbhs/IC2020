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
    public GameObject jwelPrefab;
    public bool useJewlPrefab = false;
    private List<GameObject> JwelImageObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = material[BoxTemp];
        
        if(useJewlPrefab == true)
        {
            foreach (Transform child in jwelPrefab.transform)
            {
                JwelImageObjects.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(useJewlPrefab == true)
        {
            JewelChange(BoxTemp);
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        //print(collider.gameObject.tag);
        IndividMolVelocity ColliderTemp = collider.gameObject.GetComponent<IndividMolVelocity>();
        //print("Molc temp" + ColliderTemp.temp);
        //print("Box temp" + BoxTemp);
        //print("Molc temp" + collider.gameObject.GetComponent<IndividMolVelocity>().temp);
        
        if((ColliderTemp.temp)>BoxTemp && BoxTemp < 4)
        {
            BoxTemp ++;
            //print("BoxTemp increased to" + BoxTemp);
            rend.sharedMaterial = material[BoxTemp];
            collider.gameObject.GetComponent<IndividMolVelocity>().temp --;
            //print("Molc temp decreased to" + collider.gameObject.GetComponent<IndividMolVelocity>().temp);
            
        }
        else if((ColliderTemp.temp) < BoxTemp && BoxTemp > 1)
        {
            BoxTemp --;
            //print("BoxTemp decreased to" + BoxTemp);
            rend.sharedMaterial = material[BoxTemp];
            collider.gameObject.GetComponent<IndividMolVelocity>().temp ++;

        }


    }
    
    private void JewelChange(int BoxTemp)
    {
        if(BoxTemp == 0)
        {
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
        }
        if(BoxTemp == 1)
        {
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
        }
        if(BoxTemp == 2){
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
            JwelImageObjects[1].gameObject.SetActive(true);
        }
        if(BoxTemp == 3){
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
            JwelImageObjects[1].gameObject.SetActive(true);
            JwelImageObjects[2].gameObject.SetActive(true);
        }
        if(BoxTemp == 4){
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
            JwelImageObjects[1].gameObject.SetActive(true);
            JwelImageObjects[2].gameObject.SetActive(true);
            JwelImageObjects[3].gameObject.SetActive(true);
            JwelImageObjects[4].gameObject.SetActive(true);

        }
    }
}
