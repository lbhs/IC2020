using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class numberCorrector : MonoBehaviour
{
    public InputField inputFied;
    public float max;
    public float min;

    // Update is called once per frame
    void Update()
    {
        if(inputFied.text != "" && inputFied.text != "-" && float.Parse(inputFied.text) > max && !RectTransformUtility.RectangleContainsScreenPoint(gameObject.transform.GetComponent<RectTransform>(), Input.mousePosition))
        {
            inputFied.text = max.ToString();
        }

        else if (inputFied.text != "" && inputFied.text != "-" && float.Parse(inputFied.text) < min && !RectTransformUtility.RectangleContainsScreenPoint(gameObject.transform.GetComponent<RectTransform>(), Input.mousePosition))
        {
            inputFied.text = min.ToString();
        }
    }
}
