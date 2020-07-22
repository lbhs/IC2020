using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnMouseClickUnbondHandler();
public delegate void OnMouseClickDontUnbondHandler();

public class ChangeUnbondingText : MonoBehaviour
{
    private bool unbonding;

    public event OnMouseClickUnbondHandler OnMouseClickUnbond;

    public event OnMouseClickDontUnbondHandler OnMouseClickDontUnbond;

    public static ChangeUnbondingText Instance { get; private set; }

    private void Start()
    {
        unbonding = false;

        // Singleton design pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UnbondingStateChange()
    {
        if (unbonding)
        {
            unbonding = false;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            OnMouseClickDontUnbond?.Invoke();
        }
        else
        {
            unbonding = true;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            OnMouseClickUnbond?.Invoke();
        }
    }
}
