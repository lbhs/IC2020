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

        //Ionic Lattice Model--INSOLUBLE SALT
        else if (dropDownValue == 1)
        {
            //randomly adds equal numbers of positive and negative ions (6 is default value)
			List<Vector3> coords = generateRandomCoords(numberOfEachMonoculesPerColor*2);
			int count = 0;
            for(int x = 0; x < numberOfEachMonoculesPerColor; x++)
            {
				Particle Carbonate = new Particle("Carbonate", charge: -2f, ICColor.Carbonate, coords[count], mass: 4.0f, scale: 1.0f, bounciness: 0.2f, friction: 0.02f, grav: true);
				count++;
				Particle Copper = new Particle("Copper Ion", charge: 2f, ICColor.Magnesium, coords[count], mass: 6.0f, scale: 1.0f, bounciness: 0.2f);
				count++;
				
				Carbonate.Spawn();
				Copper.Spawn();
            }

            //sets the Buffet table options, see Buffet Table > Panel > UIDropToWorld > PossibleParticles 
            //for options. Make sure to spell them exactly the same
            BuffetTable.GetComponent<UIDropToWorld>().ChangeBuffetTable("[P] Cu2+ Ion", "[P] Carbonate ion", "[P] Cl- ion", "[P] Na+ ion", "[P] Water", "[P] Water");

            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
           //Debug.Log("[DEBUG]: Spawned NaCl Ionic Lattice.");
        }

        //Ionic Bonding Model--SOLUBLE SALT
        else if (dropDownValue == 2)
        {
            //randomly adds equal numbers of positive and negative ions (6 is default value)
			List<Vector3> coords = generateRandomCoords(numberOfEachMonoculesPerColor*2);
			int count = 0;
            for(int x = 0; x < 1; x++)
            {
				Particle Chloride = new Particle("Chloride", charge: -0.7f, ICColor.Chlorine, coords[count], mass: 3.0f, scale: 1.5f, bounciness: 0.6f);
				count++;
				Particle Sodium = new Particle("Sodium Ion", charge: 0.7f, ICColor.Sodium, coords[count], mass: 2.0f, scale: 1.5f, bounciness: 0.6f);
				count++;
				
				Chloride.Spawn();
				Sodium.Spawn();
            }

            //sets the Buffet table options, see Buffet Table > Panel > UIDropToWorld > PossibleParticles 
            //for options. Make sure to spell them exactly the same
            BuffetTable.GetComponent<UIDropToWorld>().ChangeBuffetTable("[P] Cu2+ Ion", "[P] Carbonate ion", "[P] Cl- ion", "[P] Na+ ion", "[P] Water", "[P] Water");

            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
           //Debug.Log("[DEBUG]: Spawned NaCl Ionic Lattice.");
        }
        
        //Redox Reaction Scene
        else if (dropDownValue == 3)
        {
            SceneManager.LoadScene(RedoxScene);

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

            //do stuff here
            //sets the Buffet table options, see Buffet Table > Panel > UIDropToWorld > PossibleParticles 
            //for options. Make sure to spell them exactly the same
            BuffetTable.GetComponent<UIDropToWorld>().ChangeBuffetTable("Wild Card", "Wild Card", "Wild Card", "Wild Card", "Wild Card", "Wild Card");
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
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
