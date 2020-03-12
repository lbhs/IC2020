using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkLosing : MonoBehaviour
{
    public Text loseText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collider)
    {
        print(collider.gameObject.tag);

        if (collider.gameObject.tag == "ChlorineMol" || collider.gameObject.tag == "HMol")
        {
            collider.gameObject.transform.position = new Vector3(-15, 0, -15);
            print("lose");
            GameObject.Find("LoseText").GetComponent<RectTransform>().localScale = Vector3.one;
            Time.timeScale = 0;
        }
    }
}
