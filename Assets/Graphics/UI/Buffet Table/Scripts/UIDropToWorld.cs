/*
- TODO:
- Document this.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropToWorld : MonoBehaviour, IDropHandler
{
    public GameObject prefab;
    public GameObject[] prefabs;
    public GameObject[] Images;
    private Vector3 prefabWorldPosition;
    private int objectToUse;
    private GameObject MainObject;

    void Start()
    {
        MainObject = GameObject.Find("ControllerRandy"); // If the main controller object's name has been changed, edit the string within GameObject.Find()
        Debug.Log(MainObject);
    }
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform Panel = transform as RectTransform;
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
        Debug.Log(prefabWorldPosition);
        objectToUse = 2;

        if (!RectTransformUtility.RectangleContainsScreenPoint(Panel,
            Input.mousePosition))
        {
            foreach (GameObject item in Images)
            {
                if (item.GetComponent<UIDragNDrop>().UsingMe == true)
                {
                    objectToUse = int.Parse(item.name);
                    Debug.Log(objectToUse);
                }
            }
            MainObject.GetComponent<SphereSpawner>().AddSphere(Images[objectToUse].GetComponent<UIDragNDrop>().mass, Images[objectToUse].GetComponent<UIDragNDrop>().charge, Images[objectToUse].GetComponent<UIDragNDrop>().elastic, prefabWorldPosition, Images[objectToUse].GetComponent<UIDragNDrop>().color, Images[objectToUse].GetComponent<UIDragNDrop>().scale);
            Debug.Log("[DEBUG]: Created one object.");
        }
    }
}
