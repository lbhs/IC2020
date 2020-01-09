using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHCl : MonoBehaviour
{
   public GameObject AtomA;
   public GameObject AtomB;

        public void SpawnMolecule()
    {
        Instantiate(AtomA, new Vector3 (4,7,0), Quaternion.identity);
        Instantiate(AtomB);
    }
}
