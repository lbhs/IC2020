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
    public IDictionary<string, int> DefinedLimits = new Dictionary<string, int>();

    private void Start()
    {
        DefinedLimits.Add("CarbonE", 4);
        DefinedLimits.Add("CarbonEsp2", 4);
        DefinedLimits.Add("HydE", 18);
        DefinedLimits.Add("ChlorineE", 4);
        DefinedLimits.Add("OxygenE", 6);
        DefinedLimits.Add("OxygenEB", 3);
    }


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
        if (objectToUse != null)
        {
            if (PrefabCanBeDrawn(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn.name))
            {
                GameObject.Find("GameSetup").GetComponent<GameSetupContrller>().NetowrkSpawn(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn, prefabWorldPosition);
                if (objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn.name.Length >= 8)
                {
                    if (objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn.name.Substring(0, 8) == "Diatomic")
                    {
                        GameObject.Find("GameSetup").GetComponent<PhotonView>().RPC("ChangeScoreUniformly", RpcTarget.All, 0, 10);
                    }
                }
                GameObject.Find("GameSetup").GetComponent<PhotonView>().RPC("CalExit", RpcTarget.All);
            }
            else
            {
                Debug.Log(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn.name + " cannot be drawn.");
            }
        }
        //Instantiate(objectToUse.GetComponent<UIDragNDrop>().PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
        //}
    }

    private bool PrefabCanBeDrawn(string PrefabName, bool Committed = true)
    {
        bool Condition = false;
        string DecrementingKeyName = "";
        int AmountToDecrement = 0;

        if (PrefabName == "DiatomicCl2D")
        {
            Condition = DefinedLimits["ChlorineE"] >= 2;
            DecrementingKeyName = "ChlorineE";
            AmountToDecrement = 2;
        }

        else if (PrefabName == "DiatomicH2D")
        {
            Condition = DefinedLimits["HydE"] >= 2;
            DecrementingKeyName = "HydE";
            AmountToDecrement = 2;
        }

        else if (PrefabName == "DiatomicO2D")
        {
            Condition = DefinedLimits["OxygenEB"] >= 2;
            DecrementingKeyName = "OxygenEB";
            AmountToDecrement = 2;
        }

        else if (PrefabName == "OxLinearE")
        {
            Condition = DefinedLimits["OxygenE"] > 0;
            DecrementingKeyName = "OxygenE";
            AmountToDecrement = 1;
        }

        else
        {
            Condition = DefinedLimits[PrefabName] > 0;
            DecrementingKeyName = PrefabName;
            AmountToDecrement = 1;
        }

        if (Condition && Committed)
        {
            DefinedLimits[DecrementingKeyName] -= AmountToDecrement;
        }

        return Condition;
    }

}
