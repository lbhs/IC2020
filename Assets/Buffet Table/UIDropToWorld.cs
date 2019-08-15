using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using IC2020;

public class UIDropToWorld : MonoBehaviour, IDropHandler
{
    public GameObject[] prefabs;
    public GameObject[] possibleParticles;
    [Header("Ignore:")]
    public GameObject[] Images;
    public Sprite plus;
    public Sprite minus;
    public Sprite Water;
    public Sprite Sphere;
    public Sprite transparent;
    private Vector3 prefabWorldPosition;
    private int objectToUse;
    private GameObject MainObject;
    private MoleculeSpawner mSpawner = new MoleculeSpawner();

    public void ChangeBuffetTable(string[] strings)
    {
        foreach (GameObject P in possibleParticles)
        {
            foreach (string item in collection)
            {

            if(P.name == string0)
            {
                prefabs[0] = P;
                if(P.name != "[P] Water")
                {
                Images[0].GetComponent<Image>().sprite = Sphere;
                Images[0].GetComponent<Image>().color = P.GetComponent<Renderer>().material.color;
                //put charge stuff here
                }
                else
                {
                Images[0].GetComponent<Image>().sprite = Water;
                }
            }
            }
        }
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform Panel = transform as RectTransform;
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
        //Debug.Log(prefabWorldPosition);
        objectToUse = 2;

        if (!RectTransformUtility.RectangleContainsScreenPoint(Panel,
            Input.mousePosition))
        {
            foreach (GameObject item in Images)
            {
                if (item.GetComponent<UIDragNDrop>().UseingMe == true)
                {
                    objectToUse = int.Parse(item.name);
                    //Debug.Log(objectToUse);
                }
            }

            if (Images[objectToUse].GetComponent<UIDragNDrop>().useAddSphere == true)
            {
                Particle p = new Particle("BuffetParticle", Images[objectToUse].GetComponent<UIDragNDrop>().charge, Images[objectToUse].GetComponent<UIDragNDrop>().color, prefabWorldPosition, Images[objectToUse].GetComponent<UIDragNDrop>().mass, Images[objectToUse].GetComponent<UIDragNDrop>().scale, Images[objectToUse].GetComponent<UIDragNDrop>().bounciness); // Temporary name before a convention is decided on.
                p.Spawn();
            }
            else if (Images[objectToUse].GetComponent<UIDragNDrop>().useAddWater == true)
            {
                mSpawner.AddWater((float) prefabWorldPosition.x, (float) prefabWorldPosition.y);
                //MainObject.GetComponent<forces>().addWater((float)prefabWorldPosition.x, (float)prefabWorldPosition.y);
            }
            else
            {
                Instantiate(prefabs[objectToUse], prefabWorldPosition, Quaternion.identity);
            }
			//Debug.Log("[DEBUG]: created stuff");
        }
    


    }
}
