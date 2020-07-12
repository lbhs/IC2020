using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUnbondingText : MonoBehaviour
{
    public void UnbondingStateChange()
    {
        if (GameSetupContrller.Instance.Unbonding)
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
}
