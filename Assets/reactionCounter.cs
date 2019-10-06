using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reactionCounter : MonoBehaviour
{
    public int number = 0;

    void Update()
    {
        gameObject.GetComponent<Text>().text = "Reaction Count: " + number;
    }
}
