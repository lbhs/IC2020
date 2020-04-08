using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDropToWorld : MonoBehaviour, IDropHandler
{
    //  Variable Definitions

    //[HideInInspector] public bool startWithAllWildCards; //(see comment at the top of UIDragNDrop) a bool to change all tiles to wild card when standalone scenes are present
   // public GameObject[] prefabs; // the list of actual objects to be spawned 
    //[HideInInspector] public GameObject[] possibleParticles; //   (see comment at the top of UIDragNDrop) A list of objects that can be pulled from the buffet table in a specific scene.
    //[Header("Ignore:")]
   // public GameObject[] Images; // The list of UI elements that are being dragged (need to be labeled 0, 1, 2, etc. in buffet table).
    private Vector3 prefabWorldPosition; // Position that the prefab spawns in.
    private GameObject objectToUse; // Index used in prefabs[] to determine which particle is spawned.
    public  List<GameObject> Tiles = new List<GameObject>();
    //public GameObject UIObject;


    public void OnDrop(PointerEventData eventData)
    {
        // The buffet table's position
        RectTransform panel = transform as RectTransform;
        //the point where the particle should be spawned
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;


        // the 9 lines below this comment determine whether an should be spawned object depending on whether it is spawned inside or outside of the world.
        //if (!RectTransformUtility.RectangleContainsScreenPoint(panel, Input.mousePosition))
        //{ 
        foreach (GameObject item in Tiles)
        {
            if (item.GetComponent<UIDragNDrop>().UseingMe)
            {
                objectToUse = item.gameObject;
            }
        }
        GameObject.Find("GameSetup").GetComponent<GameSetupContrller>().NetowrkSpawn(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn, prefabWorldPosition);
        GameObject.Find("GameSetup").GetComponent<GameSetupContrller>().CalExit();
            //Instantiate(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn, prefabWorldPosition, Quaternion.identity);

        //}
    }

}
