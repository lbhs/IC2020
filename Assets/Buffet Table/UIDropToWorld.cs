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

    public void ChangeBuffetTable(string string0, string string1, string string2, string string3, string string4, string string5, string string6, bool useWildCard)
    {
        foreach (GameObject P in possibleParticles)
        {
            if(P.name == string0)
            {
                prefabs[0] = P;

                //sets the charge to plus or minus
                if(P.GetComponent<charger>().charge > 0)
                {
                    Images[0].transform.GetChild(0).GetComponent<Image>().sprite = plus;
                }
                else if (P.GetComponent<charger>().charge < 0)
                {
                    Images[0].transform.GetChild(0).GetComponent<Image>().sprite = minus;
                }
                else
                {
                    Images[0].transform.GetChild(0).GetComponent<Image>().sprite = transparent;
                }

                //sets the sprite and color of the particle
                if(P.name != "[P] Water")
                {
                    Images[0].GetComponent<Image>().sprite = Sphere;
                    Images[0].GetComponent<Image>().color = P.GetComponent<Renderer>().sharedMaterial.color;
                    string[] tempName = P.name.Split(']');
                    Images[0].transform.parent.transform.GetChild(1).GetComponent<Text>().text = tempName[1].TrimStart(' ');
                }
                else
                {
                    Images[0].GetComponent<Image>().sprite = Water;
                    Images[0].GetComponent<Image>().color = Color.white;
                    Images[0].transform.GetChild(0).GetComponent<Image>().sprite = transparent;
                    Images[0].transform.parent.transform.GetChild(1).GetComponent<Text>().text = "H₂0";
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
