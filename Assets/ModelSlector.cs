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
    [SerializeField]
    private int numberOfEachMonoculesPerColor = 4;
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
            // Particle Chloride = new Particle("Chloride", -2f, ICColor.Chlorine, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), 2.0f, scale: 2.0f);
            // Particle Sodium = new Particle("Sodium Ion", 2f, ICColor.Sodium, new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0), 2.0f, scale: 2.0f);
            
            ICParticles.Sulfate.Spawn(new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0));
            ICParticles.SodiumIon.Spawn(new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0));
            ICParticles.SodiumIon.Spawn(new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0));
            }
            
            dropDownMenu.GetComponent<Dropdown>().value = 0;
            pannel.SetActive(false);
            Debug.Log("[DEBUG]: Spawned NaCl Ionic Lattice.");
        }

        //Covalent Bonding Model
        else if (dropDownValue == 2)
        {
            ICParticles.Electron.Spawn(new Vector3(2, 3, 0));
            ICParticles.Electron.Spawn(new Vector3(.5f, 0, 0));

            GameObject Hyd1 = ICParticles.Hydrogen.Spawn(new Vector3(4, 1, 0));
            GameObject Hyd2 = ICParticles.Hydrogen.Spawn(new Vector3(.2f, 2, 0));
            // GameObject Hyd2 = ICParticles.Hybrogen.Spawn(new Vector3(.2f, 2, 0));

            Hyd1.transform.localScale = new Vector3(2f, 2f, 2f);
            Hyd2.transform.localScale = new Vector3(2f, 2f, 2f);
            
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
    }
}
