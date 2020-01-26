using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WildCardController : MonoBehaviour
{
    public GameObject customView;
    public GameObject prefabView;
    public GameObject currentTile;

    [Header("Images")]
    public Sprite Sphere;
    public Sprite Plus;
    public Sprite Minus;
    public Sprite Transparent;

    [Header("InputFeilds for custom menu")]
    public InputField PName;
    public InputField PMass;
    public Toggle PPercipatae;
    public InputField PCharge;
    [HideInInspector] public Color CurrentColor;
    public InputField PSize;
    public InputField PBounciness;
    public InputField PFriction;
    public Toggle[] colors;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf == true)
        {
            if (PName.text != "" && PMass.text != "" && PCharge.text != "" && PSize.text != "" && PBounciness.text != "" && PFriction.text != "")
            {
                updateWildMenu();
            }
        }
    }

    public void updateWildMenu()
    {
        //set the ui image
        currentTile.GetComponent<Image>().sprite = Sphere;  
        //sets the name
        currentTile.transform.parent.transform.GetChild(1).GetComponent<Text>().text = PName.text;
        currentTile.GetComponent<UIDragNDrop>().particleName = PName.text;
        //sets mass
        currentTile.GetComponent<UIDragNDrop>().mass = float.Parse(PMass.text);
        //sets percipate
        currentTile.GetComponent<UIDragNDrop>().precipitate = PPercipatae.isOn;
        //sets charge
        currentTile.GetComponent<UIDragNDrop>().charge = float.Parse(PCharge.text);
        if(float.Parse(PCharge.text) > 0)
        {
            currentTile.transform.GetChild(0).GetComponent<Image>().sprite = Plus;
        }
        else if (float.Parse(PCharge.text) < 0)
        {
            currentTile.transform.GetChild(0).GetComponent<Image>().sprite = Minus;
        }
        else
        {
            currentTile.transform.GetChild(0).GetComponent<Image>().sprite = Transparent;
        }
        //sets color
        GetColor();
        currentTile.GetComponent<Image>().color = CurrentColor;
        currentTile.GetComponent<UIDragNDrop>().color = CurrentColor;
        //sets size
        currentTile.GetComponent<RectTransform>().sizeDelta = new Vector2((20 * int.Parse(PSize.text)) + 5, (20 * int.Parse(PSize.text)) + 5); //converts the prefab size to ui size using y=20x+5
        currentTile.GetComponent<UIDragNDrop>().scale = int.Parse(PSize.text);
        //sets bounciness
        currentTile.GetComponent<UIDragNDrop>().bounciness = float.Parse(PBounciness.text);
        //sets friction 
        currentTile.GetComponent<UIDragNDrop>().friction = float.Parse(PFriction.text);
        //sets up logic stuff
        //currentTile.GetComponent<UIDragNDrop>().isWildCard = false; //disables wild card scripts
        currentTile.GetComponent<UIDragNDrop>().useAddShpere = true;
        currentTile.GetComponent<UIDragNDrop>().isInteractable = true;
    }

    public void GetColor()
    {
        foreach (var item in colors)
        {
            if (item.GetComponent<Toggle>().isOn == true)
            {
                CurrentColor = item.transform.GetChild(0).GetComponent<Image>().color;
            }
        }
    }

    public void PrefabButton()
    {
        customView.SetActive(false);
        prefabView.SetActive(true);
    }

    public void CustomButton()
    {
        customView.SetActive(true);
        prefabView.SetActive(false);
    }
}
