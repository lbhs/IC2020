using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleList : MonoBehaviour
{
    public GameObject particleButton;
    public GameObject previousButton;
    public Sprite plus; 
    public Sprite minus; 
    public Sprite transparent;
    private GameObject[] pP;
    // should be onEnable when done programing
    void Start()
    {
        pP = GameObject.Find("Panel").GetComponent<UIDropToWorld>().possibleParticles;
        foreach (GameObject P in pP)
        {
            if(P.name != "[P] Water")
            {
                GameObject button;
                button = Instantiate(particleButton, gameObject.transform.position, Quaternion.identity);
                button.transform.SetParent(previousButton.transform);
                Vector3 pos = button.transform.position;
                button.transform.localPosition = new Vector3(0,-30,0);
                previousButton = button;
                button.transform.localScale = new Vector3(1, 1, 1);

                if (P.GetComponent<charger>().charge > 0)
                {
                    button.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = plus;
                }
                else if (P.GetComponent<charger>().charge < 0)
                {
                    button.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = minus;
                }
                else
                {
                    button.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = transparent;
                }

                button.transform.GetChild(0).GetComponent<Image>().color = P.GetComponent<Renderer>().sharedMaterial.color;
                string[] tempName = P.name.Split(']'); // splits "[P] Particle" into "[P" and " Particle"
                button.transform.GetChild(1).GetComponent<Text>().text = tempName[1].TrimStart(' ');  //removes the space before the name
            }
        }
    }
}
