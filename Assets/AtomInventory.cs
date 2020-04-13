using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomInventory : MonoBehaviour
{
    public static List<GameObject>[] MoleculeList;
    public int[] bonusPts = new int[13];  //points for completing molecules of different sizes
    private int i;
    //private List<GameObject> Molecule3;
    //public static List<List<GameObject>> MoleculeList2;



    // Start is called before the first frame update
    void Start()
    {
                 
        MoleculeList = new List<GameObject>[12];  //limits player to 12 total molecules (seems plenty)
        //MoleculeList2 = new List<List<GameObject>>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))  //this will print the molecule lists!
        {
                                   
            for (i = 1; i < 12; i++)
            {
                print("Molecule " +i);
                
                if(MoleculeList[i]!= null)  //attempts to access an empty (null) molecule list throws an error message
                {
                    foreach (GameObject atom in MoleculeList[i]) //GameObject.Find("MoleculeListKeeper").GetComponent<//AtomInventory>().MoleculeList[Index])
                    {
                        print(atom.name);
                    }
                }
                
            }

            
        }

        /*
        if(Input.GetKeyDown("a"))

        foreach (List<GameObject> Molecule3 in MoleculeList2)
        {
            print("next molecule");
            foreach (GameObject atom in Molecule3) //GameObject.Find("MoleculeListKeeper").GetComponent<//AtomInventory>().MoleculeList[Index])
            {
                print(atom.name);
            }
        }
         */   

    }
}
