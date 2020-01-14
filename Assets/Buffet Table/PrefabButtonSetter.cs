using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabButtonSetter : MonoBehaviour
{/*
    private Button B;
    public GameObject thePrefab;
    [HideInInspector] public WildCardController WildCard;
    private UIDropToWorld buffetTable;
    

    // Start is called before the first frame update
    void Start()
    {
        WildCard = GameObject.Find("WildCardMenu").GetComponent<WildCardController>();
        B = gameObject.GetComponent<Button>();
        B.onClick.AddListener(B_onClick); //adds lisitner to the button this object is attached to, when it is pressed it calls the function b_onClick
        buffetTable = GameObject.Find("Panel").GetComponent<UIDropToWorld>();
    }

    void B_onClick()
    {
        WildCard.currentTile.GetComponent<Image>().sprite = gameObject.transform.GetChild(0).GetComponent<Image>().sprite; //image (sphere or water)
        WildCard.currentTile.GetComponent<Image>().color = gameObject.transform.GetChild(0).GetComponent<Image>().color; //color of above image
        WildCard.currentTile.transform.parent.transform.GetChild(1).GetComponent<Text>().text = gameObject.transform.GetChild(1).GetComponent<Text>().text; //sets the title
        //TODO scale and fix title bug
        WildCard.currentTile.GetComponent<UIDragNDrop>().isInteractable = true;
        buffetTable.prefabs[int.Parse(WildCard.currentTile.name)] = thePrefab;
        if (gameObject.name != "Water Button") //if it exitst (in the water one, it dosen't
        {
            WildCard.currentTile.transform.GetChild(0).GetComponent<Image>().sprite = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite; //set the charge iamge
            float size = thePrefab.transform.localScale.x;
            WildCard.currentTile.GetComponent<RectTransform>().sizeDelta = new Vector2((20 * size) + 5, (20 * size) + 5);
        }
        else
        {
            WildCard.currentTile.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        }
    }*/
}
