using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelSlector : MonoBehaviour
{
    public GameObject pannel;
    public GameObject dropDownMenu;
    private int dropDownValue;
    [Header("Ionic Lattice Model Options")]
    public int numberOfEachMonoculesPerColor;



    // Start is called before the first frame update
    void Start()
    {
        
    }
 
    // Update is called once per frame
    void Update()
    {
        dropDownValue = dropDownMenu.GetComponent<Dropdown>().value;
        ChooseModel();
    }


    public void OpenEmptyScene()
    {
        pannel.SetActive(false);
    }


    public void ChooseModel()
    {
        //value 0 is the first option, 1 is the 2ed, ect...
        if(dropDownValue == 0)
        {
            //nothing because it is the place holder text 'Choose Model'
        }

        else if(dropDownValue == 1)
        {
            //randomly adds several of 2 different kinds of particles
            for(int x = 0; x < numberOfEachMonoculesPerColor; x++)
            {
            GameObject.Find("GameObject").GetComponent<forces>().addSphere(1.0f, -2, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.blue, 1);
            GameObject.Find("GameObject").GetComponent<forces>().addSphere(2.0f, 2, true, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), Color.red, 2);
                Debug.Log("stuff");
            }
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
        }

        else if (dropDownValue == 2)
        {
            GameObject.Find("GameObject").GetComponent<forces>().addSphere(1.0f, -2, true, new Vector3(2, 3, 0), Color.blue, 1);
            GameObject.Find("GameObject").GetComponent<forces>().addSphere(1.0f, -2, true, new Vector3(0.5f, 0, 0), Color.blue, 1);
            GameObject.Find("GameObject").GetComponent<forces>().addSphere(2.0f, 2, true, new Vector3(4, 1, 0), Color.red, 2);
            GameObject.Find("GameObject").GetComponent<forces>().addSphere(2.0f, 2, true, new Vector3(0.2f, 2, 0), Color.red, 2);
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
        }
    }
}
