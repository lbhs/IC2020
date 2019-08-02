using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorChanger : MonoBehaviour
{
    public Sprite anchorImage;
    private Sprite previousImage;
    private bool hasBeenUpdated;

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<ImageFollower>().sphereToFollow.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll && hasBeenUpdated == false)
        {
            previousImage = gameObject.GetComponent<Image>().sprite;
            gameObject.GetComponent<Image>().sprite = anchorImage;
            hasBeenUpdated = true;
        }
        else if (gameObject.GetComponent<ImageFollower>().sphereToFollow.GetComponent<Rigidbody>().constraints != RigidbodyConstraints.FreezeAll && hasBeenUpdated == true)
        {
            gameObject.GetComponent<Image>().sprite = previousImage;
            hasBeenUpdated = false;
        }
    }
}
