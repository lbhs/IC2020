using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using IC2020;

public class UIDropToWorld : MonoBehaviour, IDropHandler
{
    //  Variable Definitions
    
    public GameObject[] prefabs; // the list of actual objects to be spawned 
    public GameObject[] possibleParticles; // A list of objects that can be pulled from the buffet table in a specific scene.
    [Header("Ignore:")]
    public GameObject[] Images; // The list of UI elements that are being dragged (need to be labeled 0, 1, 2, etc. in buffet table).
    public Sprite plus; // Plus symbol overlaid on UI elements.
    public Sprite minus; // Minus symbol overlaid on UI elements.
    public Sprite Water; // Water Sprite
    public Sprite Sphere; // Sphere sprite. Color is controlled by prefab material color.
    public Sprite transparent; // Transparent placeholder image when no other image is used.
    private Vector3 prefabWorldPosition; // Position that the prefab spawns in.
    private int objectToUse; // Index used in prefabs[] to determine which particle is spawned.
    private GameObject MainObject; // "Main" controller gameobject.
    private MoleculeSpawner mSpawner = new MoleculeSpawner(); // A molecule spawner object used to add water molecules.

    public void ChangeBuffetTable(string string0, string string1, string string2, string string3, string string4, string string5, string string6, bool useWildCard)
    {
        List<string> strings = new List<string>();
        strings.Add(string0);
        strings.Add(string1);
        strings.Add(string2);
        strings.Add(string3);
        strings.Add(string4);
        strings.Add(string5);
        strings.Add(string6);

        foreach (GameObject P in possibleParticles)
        {
            if (P.name == string0)
            { 
                ChangeBuffetTableAction(P,0);
            } 
             if (P.name == string1)
            {
                ChangeBuffetTableAction(P, 1);
            }
             if (P.name == string2)
            {
                ChangeBuffetTableAction(P, 2);
            }
            if (P.name == string3)
            {
                ChangeBuffetTableAction(P, 3);
            }
             if (P.name == string4)
            {
                ChangeBuffetTableAction(P, 4);
            }
            if (P.name == string5)
            {
                ChangeBuffetTableAction(P, 5);
            }
        }
    }

    private void ChangeBuffetTableAction(GameObject P, int counter)
    {

        prefabs[counter] = P;

        //sets the charge to plus or minus
        if (P.GetComponent<charger>().charge > 0)
        {
            Images[counter].transform.GetChild(0).GetComponent<Image>().sprite = plus;
        }
        else if (P.GetComponent<charger>().charge < 0)
        {
            Images[counter].transform.GetChild(0).GetComponent<Image>().sprite = minus;
        }
        else
        {
            Images[counter].transform.GetChild(0).GetComponent<Image>().sprite = transparent;
        }

        //Debug.Log("P= " + P);
        //sets the sprite and color of the particle
        if (P.name == "[P] Water")
        {
            print("waterwatrwrsefasbfgbhagj");
            Images[counter].GetComponent<Image>().sprite = Water;
            Images[counter].GetComponent<Image>().color = Color.white;
            Images[counter].GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
            Images[counter].transform.GetChild(0).GetComponent<Image>().sprite = transparent;
            Images[counter].transform.parent.transform.GetChild(1).GetComponent<Text>().text = "H₂0";
        }
        else
        {
            print("kjdbgaoeghebijcsidv");
            Images[counter].GetComponent<Image>().sprite = Sphere;
            Images[counter].GetComponent<Image>().color = P.GetComponent<Renderer>().sharedMaterial.color;
            string[] tempName = P.name.Split(']');
            Images[counter].transform.parent.transform.GetChild(1).GetComponent<Text>().text = tempName[1].TrimStart(' ');
            float size = P.transform.localScale.x;
            Images[counter].GetComponent<RectTransform>().sizeDelta = new Vector2((20 * size) + 5, (20 * size) + 5);
        }

    }

        public void OnDrop(PointerEventData eventData)
    {
        // The buffet table's position
        RectTransform panel = transform as RectTransform; 
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
        //Debug.Log(prefabWorldPosition);
        //objectToUse = 2;

        // Lines 119-127 determine whether an should be spawned object depending on whether it is spawned inside or outside of the world.
        if (!RectTransformUtility.RectangleContainsScreenPoint(panel, Input.mousePosition))
        {
            foreach (GameObject item in Images)
            {
                if (item.GetComponent<UIDragNDrop>().UseingMe) 
                {
                    objectToUse = int.Parse(item.name);
                }
            }
            
            // Spawns the actual prefab.
            Instantiate(prefabs[objectToUse], prefabWorldPosition, Quaternion.identity);
        }
    }
}

/*if (Images[objectToUse].GetComponent<UIDragNDrop>().useAddSphere == true)
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
            }*/
