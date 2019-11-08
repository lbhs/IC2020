using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IC2020;

public class ModelSlector : MonoBehaviour
{
    public GameObject pannel;
    public GameObject dropDownMenu;
    public GameObject cubePrefab;
    private int dropDownValue;
    public string RedoxScene;
    [Header("Ionic Lattice Model Options")]
    public int numberOfEachMonoculesPerColor;
    private MoleculeSpawner pSpawner = new MoleculeSpawner();
    private GameObject BuffetTable;

    //private List<Vector3> coordinates = new List<Vector3>();

    void Update()
    {
        BuffetTable = GameObject.Find("Panel");
        dropDownValue = dropDownMenu.GetComponent<Dropdown>().value;
        ChooseModel();
    }
    
    public void OpenEmptyScene()
    {
        //sets the Buffet table options, see Buffet Table > Panel > UIDropToWorld > PossibleParticles 
        //for options ("Wild Card" is also an option). Make sure to spell them exactly the same
        BuffetTable.GetComponent<UIDropToWorld>().ChangeBuffetTable("Wild Card", "Wild Card", "Wild Card", "Wild Card", "Wild Card", "Wild Card");
        pannel.SetActive(false);
    }
    
    public void ChooseModel()
    {
        //value 0 is the first option, 1 is the 2ed, ect...
        if(dropDownValue == 0)
        {
            // nothing because it is the place holder text 'Choose Model'
        }

        //ELECTRON TRANSFER Model--Battle Royal
        else if (dropDownValue == 1)
        {
            SceneManager.LoadScene("Battle Royal (activity series)");
        }

        //Redox Model--Breakout
        else if (dropDownValue == 2)
        {
            SceneManager.LoadScene("Breakout");
        }
        
        //Redox Model--Chemistry Art
        else if (dropDownValue == 3)
        {
            SceneManager.LoadScene("Penny models");

            //this is now useless because a new scene has just loaded
            //sets the Buffet table options, see Buffet Table > Panel > UIDropToWorld > PossibleParticles 
            //for options. Make sure to spell them exactly the same
            //BuffetTable.GetComponent<UIDropToWorld>().ChangeBuffetTable("Wild Card", "Wild Card", "Wild Card", "Wild Card", "Wild Card", "Wild Card");
            //dropDownMenu.GetComponent<Dropdown>().value = 0;
            //pannel.SetActive(false);
        }
        
        //...
        else if (dropDownValue == 4)
        {
            //logic here
            SceneManager.LoadScene("Tin Man Model");
            //do stuff here
            //sets the Buffet table options, see Buffet Table > Panel > UIDropToWorld > PossibleParticles 
            //for options. Make sure to spell them exactly the same
            /*BuffetTable.GetComponent<UIDropToWorld>().ChangeBuffetTable("Wild Card", "Wild Card", "Wild Card", "Wild Card", "Wild Card", "Wild Card");
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);*/
        }

    }
	
	public List<Vector3> generateRandomCoords(int n)
	{
		if(n > 49)
			n = 49;
		List<Vector3> coordinates = new List<Vector3>();
		Vector3 currentVector  = new Vector3(UnityEngine.Random.Range(-7, 7), UnityEngine.Random.Range(-7, 7), 0);
		for(int i = 0; i < n; i++)
		{
			while(coordinates.Contains(currentVector))
			{
				currentVector = new Vector3(UnityEngine.Random.Range(-7, 7), UnityEngine.Random.Range(-7, 7), 0);
			}
			coordinates.Add(currentVector);
		}
		return coordinates;
	}
}
