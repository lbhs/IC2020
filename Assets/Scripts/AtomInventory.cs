using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomInventory : MonoBehaviour
{
    [SerializeField]
    private IDictionary<string, int> DefinedLimits;
    public static AtomInventory Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        DefinedLimits = new Dictionary<string, int>();

        DefinedLimits.Add("CarbonE", 4);
        DefinedLimits.Add("CarbonEsp2", 4);
        DefinedLimits.Add("HydE", 18);
        DefinedLimits.Add("ChlorineE", 4);
        DefinedLimits.Add("OxygenEB", 3);
        DefinedLimits.Add("OxygenE", 6);

        // Singleton design pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool PrefabCanBeDrawn(string PrefabName, bool Committed = true)
    {
        // If Committed == true, then the method decrements the DefinedLimits dictionary
        // Set this flag to true if it's known that the player will not want a different prefab

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

    public void AddPrefab(string PrefabName)
    {
        if (PrefabName == "OxLinearE")
        {
            DefinedLimits["OxygenE"]++;
        }

        else
        {
            DefinedLimits[PrefabName]++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Current Inventory Status: ");
            foreach (var entry in DefinedLimits)
            {
                Debug.LogFormat("There remains {0} of {1}", entry.Value, entry.Key);
            }
        }   
    }
}
