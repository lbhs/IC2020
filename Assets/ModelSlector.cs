using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IC2020;

public class ModelSlector : MonoBehaviour
{
    public GameObject pannel;
    public GameObject dropDownMenu;
    public GameObject cubePrefab;
    private int dropDownValue;
    [Header("Ionic Lattice Model Options")]
    public int numberOfEachMonoculesPerColor;
    private MoleculeSpawner pSpawner = new MoleculeSpawner();
	//private List<Vector3> coordinates = new List<Vector3>();
    
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
            // nothing because it is the place holder text 'Choose Model'
        }

        //Ionic Lattice Model
        else if (dropDownValue == 1)
        {
            //randomly adds several of 2 different kinds of particles
			List<Vector3> coords = generateRandomCoords(numberOfEachMonoculesPerColor*2);
			int count = 0;
            for(int x = 0; x < numberOfEachMonoculesPerColor; x++)
            {
				Particle Chloride = new Particle("Chloride", -2f, ICColor.Chlorine, coords[count], 2.0f, scale: 2.0f);
				count++;
				Particle Sodium = new Particle("Sodium Ion", 2f, ICColor.Sodium, coords[count], 2.0f, scale: 2.0f);
				count++;
				
				Chloride.Spawn();
				Sodium.Spawn();
            }
            
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
            Debug.Log("[DEBUG]: Spawned NaCl Ionic Lattice.");
        }

        //Covalent Bonding Model
        else if (dropDownValue == 2)
        {
            Particle Elec1 = new Particle("Electron 1", -2f, ICColor.Electron, new Vector3(2, 3, 0));
            Particle Elec2 = new Particle("Electron 2", -2f, ICColor.Electron, new Vector3(0.5f, 0, 0));
            Particle Hyd1 = new Particle("Hydrogen 1", 2f, ICColor.Hydrogen, new Vector3(4, 1, 0), scale: 2f);
            Particle Hyd2 = new Particle("Hydrogen 2", 2f, ICColor.Hydrogen, new Vector3(0.2f, 2, 0), scale: 2f);

            Elec1.Spawn();
            Elec2.Spawn();
            Hyd1.Spawn();
            Hyd2.Spawn();
            Instantiate(cubePrefab, new Vector3(5, 5, 0), Quaternion.identity);
            
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
            Debug.Log("[DEBUG]: Spawned H2 Covalent Bonding Model.");
        }
        
        //Na+ in Water
        else if (dropDownValue == 3)
        {
            Particle Sodium = new Particle("Sodium", 1f, ICColor.Sodium, new Vector3(0, 0, 0), mass:2.0f, scale: 2.0f);
            
            Sodium.Spawn();
            pSpawner.AddWater(0, 4);
            pSpawner.AddWater(5, 0);
            pSpawner.AddWater(0, -5);
            pSpawner.AddWater(-5, 2);
            Instantiate(cubePrefab, new Vector3(5, 5, 0), Quaternion.identity);
            
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
        }
        
        //...
        else if (dropDownValue == 4)
        {
            //do stuff here
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
        }
        FindObjectOfType<forces>().Update();
        FindObjectOfType<forces>().CalculateForces(false);
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
