using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUnbondingText : MonoBehaviour
{
    private ComponentData CD;

    // Start is called before the first frame update
    void Start()
    {
        CD = GameObject.Find("ComponentReferences").GetComponent<ComponentData>();
    }

    public void UnbondingStateChange()
    {
        if (CD.GSC.Unbonding)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
