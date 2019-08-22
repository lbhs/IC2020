using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCardController : MonoBehaviour
{
    public GameObject customView;
    public GameObject prefabView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
