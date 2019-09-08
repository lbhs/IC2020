using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabButtonSetter : MonoBehaviour
{
    private Button B;
    public GameObject thePrefab;

    // Start is called before the first frame update
    void Start()
    {
        B = gameObject.GetComponent<Button>();
        B.onClick.AddListener(B_onClick); //adds lisitner to the button this object is attached to, when it is pressed it calls the function b_onClick
    }

    void B_onClick()
    {
        Debug.Log(thePrefab);
    }
}
