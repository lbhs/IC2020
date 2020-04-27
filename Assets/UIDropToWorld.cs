using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDropToWorld : MonoBehaviour, IDropHandler
{
    //  Variable Definitions

    
   // public GameObject[] Images; // The list of UI elements that are being dragged (need to be labeled 0, 1, 2, etc. in buffet table).
    private Vector3 prefabWorldPosition; // Position that the prefab spawns in.
    private GameObject objectToUse; // Index used in prefabs[] to determine which particle is spawned.
    public  List<GameObject> Tiles = new List<GameObject>();
    


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
        //GameObject.Find("GameSetup").GetComponent<GameSetupContrller>().NetowrkSpawn(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn, prefabWorldPosition);
        GameObject.Find("UI").GetComponent<Animator>().SetTrigger("Exit");
            Instantiate(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn, prefabWorldPosition, Quaternion.identity);

        //}
    }

}
