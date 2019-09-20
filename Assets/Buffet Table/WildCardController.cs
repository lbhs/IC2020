using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WildCardController : MonoBehaviour
{
    public GameObject customView;
    public GameObject prefabView;
    public GameObject currentTile;

    [Header("InputFeilds for custom menu")]
    public InputField PName;
    public InputField PMass;
    public Toggle PPercipatae;
    public InputField PCharge;
    [HideInInspector] public Color Pcolor;
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
        
    }

    public void updateWildMenu()
    {
        //currentTile.transform.parent.GetChild(1).GetComponent<Text>().text = currentTile.GetComponent<UIDragNDrop>().particleName;
        // look at uidroptoworld lines 104-110
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
