using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomInventory : MonoBehaviour
{
    public static List<GameObject>[] MoleculeList;
    public int[] bonusPts = new int[8];  //points for completing molecules of different sizes
    private int i;
    



    // Start is called before the first frame update
    void Start()
    {
                 
        MoleculeList = new List<GameObject>[13];  //limits player to 12 total molecules bc index starts at 1 (seems plenty)
         
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))  //this will print the molecule lists!
        {
                                   
            for (i = 1; i < 12; i++)
            {
                print("Molecule " +i);
                
                if(MoleculeList[i] != null)  //attempts to access an empty (null) molecule list throws an error message
                {
                    foreach (GameObject atom in MoleculeList[i]) //GameObject.Find("MoleculeListKeeper").GetComponent<//AtomInventory>().MoleculeList[Index])
                    {
                        print(atom.name);
                    }
                }
                
            }

            
        }

       

    }
}
