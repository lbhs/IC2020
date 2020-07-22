using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

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

    private void Start()
    {

    }


    public void OnDrop(PointerEventData eventData)
    {
        // The buffet table's position
        RectTransform panel = transform as RectTransform;
        //the point where the particle should be spawned
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;

        foreach (GameObject item in Tiles)
        {
            if (item.GetComponent<UIDragNDrop>().UseingMe)
            {
                objectToUse = item.gameObject;
            }
        }

        //if (objectToUse != null)
        //{
        //    GameObject GO = PhotonNetwork.Instantiate(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn.name, prefabWorldPosition, Quaternion.identity);
        //    if (GO.tag == "Diatomic")
        //    {
        //        JouleDisplayController.Instance.GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, 0, GO.GetComponent<PhotonView>().ViewID, 10);
        //    }
        //    GameSetupContrller.Instance.GetComponent<PhotonView>().RPC("CalExit", RpcTarget.All);
        //}
    } 

}
