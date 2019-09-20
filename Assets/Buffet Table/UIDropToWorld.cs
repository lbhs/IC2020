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
    public Sprite WildCardImage; //image for wild card tiles before it was set
    private Vector3 prefabWorldPosition; // Position that the prefab spawns in.
    private int objectToUse; // Index used in prefabs[] to determine which particle is spawned.
    private GameObject MainObject; // "Main" controller gameobject.
    private MoleculeSpawner mSpawner = new MoleculeSpawner(); // A molecule spawner object used to add water molecules.

    public void ChangeBuffetTable(string string0, string string1, string string2, string string3, string string4, string string5) // see Buffet Table > Panel > UIDropToWorld > PossibleParticles for options. Make sure to spell them exactly the same                                                                                                                               
    {
        //checks if any of the input strings match or it is a wild card then it calls the right function
        //I know this looks like a job for foreach loops, but i couldnt get it to work, so if you can figure it out, do it!
        foreach (GameObject P in possibleParticles)
        {
            if (P.name == string0)
            { 
                ChangeBuffetTableAction(P,0);
            }
            else if (string0 == "Wild Card") { ChangeBuffetTableWild(0); }

            if (P.name == string1)
            {
                ChangeBuffetTableAction(P, 1);
            }
            else if (string0 == "Wild Card") { ChangeBuffetTableWild(1); }

            if (P.name == string2)
            {
                ChangeBuffetTableAction(P, 2);
            }
            else if (string0 == "Wild Card") { ChangeBuffetTableWild(2); }

            if (P.name == string3)
            {
                ChangeBuffetTableAction(P, 3);
            }
            else if (string0 == "Wild Card") { ChangeBuffetTableWild(3); }

            if (P.name == string4)
            {
                ChangeBuffetTableAction(P, 4);
            }
            else if (string0 == "Wild Card") { ChangeBuffetTableWild(4); }

            if (P.name == string5)
            {
                ChangeBuffetTableAction(P, 5);
            }
            else if (string0 == "Wild Card") { ChangeBuffetTableWild(5); }
        }
    }

    private void ChangeBuffetTableAction(GameObject P, int counter)
    {

        //sets the particle given (the prefab Water for example) to be able to be spawned
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

        
        //sets the sprite, color, and size of the buffet table image according to how the prefab looks
        if (P.name == "[P] Water")
        {
            Images[counter].GetComponent<Image>().sprite = Water; //image of a water monocule
            Images[counter].GetComponent<Image>().color = Color.white; //white will be the true color of the image, anything else will show up as a tint
            Images[counter].GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40); //size of image to be dragged
            Images[counter].transform.GetChild(0).GetComponent<Image>().sprite = transparent; //no plus or minus
            Images[counter].transform.parent.transform.GetChild(1).GetComponent<Text>().text = "H₂0"; //name of buffet table particle
            Images[counter].GetComponent<UIDragNDrop>().isWildCard = false; //disables wild card scripts
        }
        else
        {
            Images[counter].GetComponent<Image>().sprite = Sphere; //normal round sphere 
            Images[counter].GetComponent<Image>().color = P.GetComponent<Renderer>().sharedMaterial.color; //sets the color to 
            string[] tempName = P.name.Split(']'); // splits "[P] Particle" into "[P" and " Particle"
            Images[counter].transform.parent.transform.GetChild(1).GetComponent<Text>().text = tempName[1].TrimStart(' '); //removes the space before the name
            float size = P.transform.localScale.x; //sets the size of the prefab
            Images[counter].GetComponent<RectTransform>().sizeDelta = new Vector2((20 * size) + 5, (20 * size) + 5); //converts the prefab size to ui size using y=20x+5
            Images[counter].GetComponent<UIDragNDrop>().isWildCard = false; //disables wild card scripts
        }


    }

    private void ChangeBuffetTableWild(int counter)
    {
        Images[counter].GetComponent<UIDragNDrop>().isWildCard = true; //enables wild card scripts
        Images[counter].GetComponent<UIDragNDrop>().isInteractable = false; //diables dragging until presets are set
        Images[counter].GetComponent<Image>().color = Color.white; //white will be the true color of the image, anything else will show up as a tint
        Images[counter].GetComponent<Image>().sprite = WildCardImage; //image of a wild card
        Images[counter].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 60); //size of image
    }

        public void OnDrop(PointerEventData eventData)
        {
        // The buffet table's position
        RectTransform panel = transform as RectTransform; 
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
        //Debug.Log(prefabWorldPosition);
        //objectToUse = 2;
        //Debug.Log("?");
        // the 9 lines below this comment determine whether an should be spawned object depending on whether it is spawned inside or outside of the world.
        if (!RectTransformUtility.RectangleContainsScreenPoint(panel, Input.mousePosition))
        {
            foreach (GameObject item in Images)
            {
                if (item.GetComponent<UIDragNDrop>().UseingMe) 
                {
                    objectToUse = int.Parse(item.name);
                }
            }
            //Debug.Log("nope!");
            if (Images[objectToUse].GetComponent<UIDragNDrop>().useAddShpere == true)
            {
                //if its a wild card, instanciate with custom varibles
                Particle p = new Particle(Images[objectToUse].GetComponent<UIDragNDrop>().particleName, Images[objectToUse].GetComponent<UIDragNDrop>().charge, Images[objectToUse].GetComponent<UIDragNDrop>().color, prefabWorldPosition, Images[objectToUse].GetComponent<UIDragNDrop>().mass, Images[objectToUse].GetComponent<UIDragNDrop>().scale, Images[objectToUse].GetComponent<UIDragNDrop>().bounciness, Images[objectToUse].GetComponent<UIDragNDrop>().precipitate, Images[objectToUse].GetComponent<UIDragNDrop>().friction); // Temporary name before a convention is decided on. add friction+perciptates
                p.Spawn();
                Debug.Log("nope");
            }
            else
            {
                // Spawns the actual prefab.
                Debug.Log("yep");
                Instantiate(prefabs[objectToUse], prefabWorldPosition, Quaternion.identity);
            }
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
