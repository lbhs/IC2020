using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IC2020;
using System;

public class NuclearSimulation : MonoBehaviour
{
    private List<Vector3> StartPosition = new List<Vector3>();
    private List<GameObject> CurrentObjects = new List<GameObject>();
    private List<GameObject> CurrentlyInactive = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Going back to the first design: fixing the starting positions of the protons
        StartPosition.Add(new Vector3(-7f, 7f));
        StartPosition.Add(new Vector3(-7f, -7f));
        StartPosition.Add(new Vector3(7f, 7f));

        // Also the first solution: giving each proton an incredible mass
        for (int x = 0; x < 3; x++)
        {
            Particle Proton = new Particle("Hydrogen " + x, 2f, ICColor.Hydrogen, mass: 100000f, scale: 2f);
            GameObject ProtonGO = Proton.Spawn();
            ProtonGO.transform.position = StartPosition[x % 3];
            ProtonGO.AddComponent<MoleculeType>();
            CurrentObjects.Add(ProtonGO);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        for (int AIdx = 0; AIdx < CurrentObjects.Count; AIdx++)
        {
            for (int BIdx = 0; BIdx < CurrentObjects.Count; BIdx++)
            {
                GameObject a = CurrentObjects[AIdx];
                GameObject b = CurrentObjects[BIdx];
                if (a != b)
                {
                    // improvised collision detection
                    float ObjRadius = a.GetComponent<SphereCollider>().radius * a.transform.localScale.x;    // sphere must have identical scale in each direction
                    if (Math.Abs(Vector3.Distance(a.transform.position, b.transform.position) - 2 * ObjRadius) <= .25f)
                    {
                        // Scenario 1: Two protons, not part of any molecule, have collided
                        if (a.name.Split(' ')[1] == "Hydrogen" && b.name.Split(' ')[1] == "Hydrogen" &&
                            a.GetComponent<MoleculeType>().ParticleType == null && b.GetComponent<MoleculeType>().ParticleType == null)
                        {
                            // Join the protons to form deuterium
                            a.AddComponent<FixedJoint>().connectedBody = b.GetComponent<Rigidbody>();
                            a.GetComponent<MoleculeType>().ParticleType = "Deuterium";
                            b.GetComponent<MoleculeType>().ParticleType = "Deuterium";
                            RemoveLableFollower(b);
                            AddLabel(b, 3);
                            ChangeColor(0);
                        }

                        // Scenario 2: A particle in deuterium and a free proton have collided
                        else if (a.GetComponent<MoleculeType>().ParticleType == "Deuterium" && b.GetComponent<MoleculeType>().ParticleType == null)
                        {
                            a.GetComponent<MoleculeType>().ParticleType = "3Helium";
                            b.GetComponent<MoleculeType>().ParticleType = "3Helium";
                            a.AddComponent<FixedJoint>().connectedBody = b.GetComponent<Rigidbody>();

                            // The particle doesn't have the FixedJoint component itself 
                            // This is necessary to identify the other particle that comprises deuterium
                            if (!(a.HasComponent<FixedJoint>()))
                            {
                                foreach (GameObject GO in CurrentObjects)
                                {
                                    if (GO != a && GO != b)
                                    {
                                        GO.GetComponent<MoleculeType>().ParticleType = "3Helium";
                                    }
                                }
                            }
                            else
                            {
                                a.GetComponent<FixedJoint>().connectedBody.gameObject.GetComponent<MoleculeType>().ParticleType = "3Helium";
                            }
                            ChangeColor(1);
                        }
                    }
                }
            }
        }
        
    }

    private void RemoveLableFollower(GameObject ObjToRemoveLabel)
    {
        Transform ChildrenList = GameObject.Find("Lable Canvas").transform;
        for (int x = 0; x < ChildrenList.childCount; x++)
        {
            Transform child = ChildrenList.GetChild(x);
            if (child.gameObject.GetComponent<ImageFollower>().sphereToFollow == ObjToRemoveLabel)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void AddLabel(GameObject ObjToAddLabel, int LabelIdx)
    {
        if (LabelIdx < (GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs.Length))
        {
            GameObject Lable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[LabelIdx], Vector3.zero, Quaternion.identity);
            Lable.transform.parent = GameObject.Find("Lable Canvas").transform;
            Lable.AddComponent<ImageFollower>().sphereToFollow = ObjToAddLabel;
        }
    }

    private void ChangeColor(int MoleculeColorIdx)
    {
        for (int x = 0; x < CurrentObjects.Count; x++)
        {
            GameObject GO = CurrentObjects[x];
            if (GO.GetComponent<MoleculeType>().ParticleType == "Deuterium" && MoleculeColorIdx == 0)
            {
                GO.GetComponent<Renderer>().material.color = ICColor.Deuterium;
            }

            else if (GO.GetComponent<MoleculeType>().ParticleType == "3Helium" && MoleculeColorIdx == 1)
            {
                GO.GetComponent<Renderer>().material.color = ICColor.HeliumIsotope;
            }
        }

    }
}

/* Where to go from here (March 1st)
 * When the '3Helium' isotope is formed, it needs to disappear from the screen
 *      It will reappear after the second one has formed
 * Obviously, the neutrons are still identified as protons (change particle color and symbol) [Done, March 2nd]
 
 * Bugs!
 * Currently, when initializing two particles in the simulation, when they collide, they remain stationary.
 * Many foreach loops are still in use. Since CurrentObjects is being modified, this will not work. 
 * Add method documentation.
*/
