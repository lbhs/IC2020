using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool returnToZero = false; //default value is false
    public GameObject PrefabToSpawn;
    private Vector3 prefabWorldPosition;

     public bool UseingMe;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        UseingMe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (returnToZero == true)
        {
            transform.localPosition = Vector3.zero;
        }
        UseingMe = false;
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;
        Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
        if(PrefabToSpawn.tag == "Diatomic")
        {
            DisplayJoules.BonusPointTotal += 10;  //if PREFAB TO SPAWN IS DIATOMIC, GIVE 10 BONUS POINTS (no need to add to MOLECULEID LIST?)
        }
        
        
        GameObject.Find("UI").GetComponent<Animator>().SetTrigger("Exit");
    }
}