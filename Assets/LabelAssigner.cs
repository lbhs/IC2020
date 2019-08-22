using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelAssigner : MonoBehaviour
{
    // Start is called before the first frame update
    private int imgToUse;
    private GameObject tempLable;
    void OnEnable()
    {
        if (gameObject.GetComponent<charger>().charge < 0) imgToUse = 1;
            else imgToUse = 0;
                tempLable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[imgToUse], Vector3.zero, Quaternion.identity);
                tempLable.transform.SetParent(GameObject.Find("Lable Canvas").transform);
                tempLable.GetComponent<ImageFollower>().sphereToFollow = gameObject;
        //.Log(imgToUse);
    }

}
