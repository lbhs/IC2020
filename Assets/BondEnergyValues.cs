using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondEnergyValues : MonoBehaviour
{
    public int[,] bondEnergyArray;
    public int Atom1;
    public int Atom2;
    


    // Start is called before the first frame update
    void Start()
    {
        

        //H = 0, C = 1, O = 2, Cl = 3, C with double bond = 4, O with double bond = 5
        bondEnergyArray = new int[6, 6];
        bondEnergyArray[0, 0] = 2;
        bondEnergyArray[0, 1] = 2;
        bondEnergyArray[0, 2] = 3;
        bondEnergyArray[0, 3] = 2;
        bondEnergyArray[1, 0] = 2;
        bondEnergyArray[1, 1] = 2;
        bondEnergyArray[1, 2] = 2;
        bondEnergyArray[1, 3] = 2;
        bondEnergyArray[2, 0] = 3;
        bondEnergyArray[2, 1] = 2;
        bondEnergyArray[2, 2] = 1;
        bondEnergyArray[2, 3] = 1;
        bondEnergyArray[3, 0] = 2;
        bondEnergyArray[3, 1] = 2;
        bondEnergyArray[3, 2] = 1;
        bondEnergyArray[3, 3] = 1;
        bondEnergyArray[4, 4] = 3;
        bondEnergyArray[4, 5] = 4;
        bondEnergyArray[5, 4] = 4;
        bondEnergyArray[5, 5] = 3;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }
}
