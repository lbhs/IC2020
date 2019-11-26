using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    [HideInInspector] public ReactionController RC;
    void Start()
    {
        RC = GameObject.Find("ReactionManager").GetComponent<ReactionController>();
        TriggerList.Add(gameObject.GetComponent<Collider>()); //sets up the reference to the equations
    }


    List<Collider> TriggerList = new List<Collider>(); //a list of all the objects currently nearby the particle 
    //called when something enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        //if the object is not already in the list
        if (!TriggerList.Contains(other) && other.transform.root.name.Contains("[M]") || other.transform.root.name.Contains("[P]"))
        {
            //add the object to the list
            TriggerList.Add(other);
            //Debug.Log(other);
        }
    }

    //called when something exits the trigger
    private void OnTriggerExit(Collider other)
    {
        //if the object is in the list
        if (TriggerList.Contains(other) && other.transform.root.name.Contains("[M]") || other.transform.root.name.Contains("[P]"))
        {
            //remove it from the list
            TriggerList.Remove(other);
        }
    }


    void Update()
    {
        foreach (var R in RC.Reactions)
        {
            //Debug.Log("Start----------------------------------------");
            int Number = 0;
            List<GameObject> Pos = new List<GameObject>();
            Pos.Add(gameObject);
            foreach (var O in TriggerList)
            {
                foreach  (var RO in R.reactants)
                {
                    //Debug.Log(O.gameObject.transform.root.name + "     "  + RO.gameObject.name);
                    if(O.gameObject.transform.root.name == RO.gameObject.name && !Pos.Contains(O.gameObject))
                    {
                        Number++;
                        Pos.Add(O.gameObject);
                        //Debug.Log("plus one");
                        break;
                    }
                }
                Debug.Log(Number + "           " + R.reactants.Length + "            " + gameObject);
                //code for products
                if(Number == R.reactants.Length)
                {
                    int Count = 0;
                    foreach (var P in R.products)
                    {
                        Count++;
                        if (Count >= Pos.Count || Count < 0)
                        {
                            Count = 0;
                        }
                        Instantiate(P, Pos[Count].transform.position, Quaternion.identity);
                    }

                    foreach (var P in Pos)
                    {
                        Destroy(P.transform.root.gameObject);
                    }
                    //Debug.Log("Reaction!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
                //Debug.Log("ENDDDD------------------------------------------------------");
            }
           
        }
    }
}
