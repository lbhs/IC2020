using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropToWorld : MonoBehaviour, IDropHandler
{
    public GameObject prefab;
    public GameObject[] prefabs;
    public GameObject[] Images;
    private Vector3 prefacWorldPosition;
    private int objectToUse;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform Panel = transform as RectTransform;
        prefacWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefacWorldPosition.z = 0;
        Debug.Log(prefacWorldPosition);
        objectToUse = 2;

      
        /*if(GameObject.Find(prefabs) && objectInQueston.GetComponent<UIDragNDrop>.UseingMe = true)
        {
            objectToUse = int.Parse(objectInQueston.name);
        }*/

        if (!RectTransformUtility.RectangleContainsScreenPoint(Panel,
            Input.mousePosition))
        {
            foreach (GameObject item in Images)
            {
                if (item.GetComponent<UIDragNDrop>().UseingMe == true)
                {
                    objectToUse = int.Parse(item.name);
                    Debug.Log(objectToUse);
                }
            }
            Instantiate(prefabs[objectToUse], prefacWorldPosition, Quaternion.identity);
            Debug.Log("created stuff");
        }



    }
}
