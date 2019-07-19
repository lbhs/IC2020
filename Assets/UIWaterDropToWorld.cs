using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWaterDropToWorld : MonoBehaviour, IDropHandler
{
    public GameObject prefab;
    public GameObject[] prefabs;
    public GameObject[] Images;
    private Vector3 prefabWorldPosition;
    private int objectToUse;
    private GameObject MainObject;

    void Start()
    {
        MainObject = GameObject.Find("GameObject");
    }
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform Panel = transform as RectTransform;
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
        Debug.Log(prefabWorldPosition);
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
            //MainObject.GetComponent<forces>().addWater((float)prefabWorldPosition.x, (float)prefabWorldPosition.y);
            MainObject.GetComponent<forces>().addSphere(Images[objectToUse].GetComponent<UIDragNDrop>().mass, Images[objectToUse].GetComponent<UIDragNDrop>().charge, Images[objectToUse].GetComponent<UIDragNDrop>().elastic, prefabWorldPosition, Images[objectToUse].GetComponent<UIDragNDrop>().color, Images[objectToUse].GetComponent<UIDragNDrop>().scale);
            Instantiate(prefabs[objectToUse], prefabWorldPosition, Quaternion.identity);
            Debug.Log("created stuff");
        }
    


    }
}