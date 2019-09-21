using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelAssigner : MonoBehaviour
{
    public bool useTextLable;
    public string Lable;
    private int imgToUse;
    private GameObject tempLable;

    void OnEnable()
    {
        if (!useTextLable)
        {
            if (gameObject.GetComponent<charger>().charge < 0) imgToUse = 1;
            else imgToUse = 0;
            tempLable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[imgToUse], Vector3.zero, Quaternion.identity);
            tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
            tempLable.GetComponent<ImageFollower>().sphereToFollow = gameObject;
            //.Log(imgToUse);
        }
        else
        {
            tempLable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().TextPrefab, Vector3.zero, Quaternion.identity);
            tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
            tempLable.GetComponent<ImageFollower>().sphereToFollow = gameObject;
            tempLable.GetComponent<Text>().text = Lable;
        }
    }

}
