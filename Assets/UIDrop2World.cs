using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDrop2World : MonoBehaviour, IDropHandler
{
    //  Variable Definitions


    private Vector3 prefabWorldPosition; // Position that the prefab spawns in.
    public GameObject PrefabToSpawn;
    //private GameObject Prefab1;
    //private GameObject objectToUse; // Index used in prefabs[] to determine which particle is spawned.
    //public List<GameObject> Tiles = new List<GameObject>();
    //public GameObject UIObject;


    public void OnDrop(PointerEventData eventData)
    {
        //Prefab1 = gameObject.GetComponent<UIDragNDrop>().PrefabToSpawn;
        // The buffet table's position
        RectTransform panel = transform as RectTransform;
        //the point where the particle should be spawned
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
        Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
        Debug.Log("Change2World");

        // the 9 lines below this comment determine whether an should be spawned object depending on whether it is spawned inside or outside of the world.

       /* foreach (GameObject item in Tiles)
        {
            if (item.GetComponent<UIDragNDrop>().UseingMe)
            {
                objectToUse = item.gameObject;
            }
        }
        */
        //GameObject.Find("GameSetup").GetComponent<GameSetupContrller>().NetowrkSpawn(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn, prefabWorldPosition);
        //GameObject.Find("UI").GetComponent<Animator>().SetTrigger("Exit");
        //Instantiate(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn, prefabWorldPosition, Quaternion.identity);

        //}
    }

}
