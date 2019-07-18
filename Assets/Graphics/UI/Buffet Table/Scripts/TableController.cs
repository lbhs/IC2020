/*
- TODO:
- Document this.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{

    public bool isHidden;

    // Start is called before the first frame update
    void Start()
    {
        isHidden = false;
    }

    public void ToggleHide()
    {
        if(isHidden == false)
        {
            this.GetComponent<Animator>().SetTrigger("changed");
            this.GetComponent<Animator>().SetBool("isVisable",false);
            isHidden = true;
        }
        else
        {
            this.GetComponent<Animator>().SetTrigger("changed");
            this.GetComponent<Animator>().SetBool("isVisable", true);
            isHidden = false;
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }*/
}
