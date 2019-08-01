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
            for(int x = 0; x < numberOfEachMonoculesPerColor; x++)
            { 
            Particle Negative = new Particle("Anion", -2f, Color.blue, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), 2.0f, scale: 2.0f);
            Particle Positive = new Particle("Ion", 2f, Color.red, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), 2.0f, scale: 2.0f);
            
            Negative.Spawn();
            Positive.Spawn();
            }
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
            Debug.Log("[DEBUG]: Spawned Ionic Lattice.");
        }

        //Covalent Bonding Model
        else if (dropDownValue == 2)
        {
            Particle Neg1 = new Particle("Negative 1", -2f, Color.blue, new Vector3(2, 3, 0));
            Particle Neg2 = new Particle("Negative 2", -2f, Color.blue, new Vector3(0.5f, 0, 0));
            Particle Pos1 = new Particle("Positive 1", 2f, Color.red, new Vector3(4, 1, 0), scale: 2f);
            Particle Pos2 = new Particle("Positive 2", 2f, Color.red, new Vector3(0.2f, 2, 0), scale: 2f);

            Neg1.Spawn();
            Neg2.Spawn();
            Pos1.Spawn();
            Pos2.Spawn();
            Instantiate(cubePrefab, new Vector3(5, 5, 0), Quaternion.identity);
            
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
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
            
            Debug.Log("[DEBUG]: before");
            Instantiate(cubePrefab, new Vector3(5, 5, 0), Quaternion.identity);
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
            Debug.Log("[DEBUG]: after");
        }
        
        //...
        else if (dropDownValue == 4)
        {
            //do stuff here
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
        }
    }
}
