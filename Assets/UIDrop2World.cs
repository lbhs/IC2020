using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDrop2World : MonoBehaviour, IDropHandler
{
    //THIS SCRIPT IS ATTACHED TO THE JEWEL (JOULE) UI OBJECT (used for unbonding)
    
    //  Variable Definitions
    private Vector3 prefabWorldPosition; // Position that the prefab spawns in.
    public GameObject PrefabToSpawn;
    


    public void OnDrop(PointerEventData eventData)
    {
        
        RectTransform panel = transform as RectTransform;
        
        //the point where the particle should be spawned
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
		Debug.Log(gameObject);
        Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
                
    }

}
