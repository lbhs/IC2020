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
        List<string> strings = new List<string>();
        strings.Add(string0);
        strings.Add(string1);
        strings.Add(string2);
        strings.Add(string3);
        strings.Add(string4);
        strings.Add(string5);
        strings.Add(string6);
        //Debug.Log("string0 " + strings[0]);
        //Debug.Log(strings[1]);

        int counter = 0;

        foreach (GameObject P in possibleParticles)
        {
            counter = 0;
            foreach (string S in strings)
            {
                Debug.Log("P name : " + P.name);
                //Debug.Log("current string: " + S);
                //Debug.Log("P name: " + P.name);
                if (P.name == S)
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
                    if(P.name == "[P] Water")
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
                    break;
                }
                else
                {
                    if (counter <= 6)
                    {
                        counter++;
                        Debug.Log(counter);
                        //Debug.Log(counter);
                        //Debug.Log(strings);
                    }
                    else
                    {
                        break;
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
        //objectToUse = 2;

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
            {*/
                Instantiate(prefabs[objectToUse], prefabWorldPosition, Quaternion.identity);
            //}
			//Debug.Log("[DEBUG]: created stuff");
        }
    


    }
}
