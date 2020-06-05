using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUnbondingText : MonoBehaviour
{
    GameSetupContrller GSC;

    // Start is called before the first frame update
    void Start()
    {
        GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();
    }

    public void UnbondingStateChange()
    {
        if (GSC.Unbonding)
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
