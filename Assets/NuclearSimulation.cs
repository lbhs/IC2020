using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IC2020;
using System;

public class NuclearSimulation : MonoBehaviour
{
    private List<Vector3> StartPosition = new List<Vector3>();
    public List<GameObject> CurrentObjects = new List<GameObject>();
    public List<GameObject> CurrentlyInactive = new List<GameObject>();
    bool SecondRound = false;

    // Start is called before the first frame update
    void Start()
    {
        // Going back to the first design: fixing the starting positions of the protons
        StartPosition.Add(new Vector3(-7f, 7f));
        StartPosition.Add(new Vector3(0f, -7f));
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
                    // use Unity collision detection
                    // Use tags (assign in inspector)
                    // find a way to release a fixed joint
                    // use prefab for Helium-3 collision
                    // use parent-child relationship to create a multi-particle prefab
                    // add a FixedJoint component to ensure that it behaves correctly
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
                            b.GetComponent<charger>().charge = 0f;
                            DestroyLableFollower(b);
                            AddLabel(b, 3);
                            ChangeColor(0);

                            Particle Neutrino = new Particle("Neutrino", 0f, ICColor.Neutrino, mass: 0f);
                            GameObject NeutrinoGO = Neutrino.Spawn();
                            DestroyLableFollower(NeutrinoGO);
                            AddLabel(NeutrinoGO, 3);
                            Particle Beta = new Particle("Beta", 2f, ICColor.Electron, mass: 0f);
                            GameObject BetaGO = Beta.Spawn();
                            StartCoroutine(DeleteBetaNeutrino(BetaGO, NeutrinoGO));
                        }

                        // Scenario 2: A particle in deuterium and a free proton have collided
                        else if (a.GetComponent<MoleculeType>().ParticleType == "Deuterium" && b.GetComponent<MoleculeType>().ParticleType == null)
                        {
                            a.GetComponent<MoleculeType>().ParticleType = "3Helium";
                            b.GetComponent<MoleculeType>().ParticleType = "3Helium";
                            a.AddComponent<FixedJoint>().connectedBody = b.GetComponent<Rigidbody>();

                            for (int idx = 0; idx < CurrentObjects.Count; idx++)
                            {
                                GameObject GO = CurrentObjects[idx];
                                if (GO != a && GO != b)
                                {
                                    GO.GetComponent<MoleculeType>().ParticleType = "3Helium";
                                }
                            }
            
                            ChangeColor(1);
                            Particle Gamma = new Particle("Gamma", 0f, ICColor.Neutrino, mass: 0f);
                            GameObject GammaGO = Gamma.Spawn();
                            DestroyLableFollower(GammaGO);
                            AddLabel(GammaGO, 4, true, new Vector3(3f, 3f));
                            StartCoroutine(DeleteGamma(GammaGO));
                            StartCoroutine("ResetNew3Helium");
                        }
                 
                        else if (a.GetComponent<MoleculeType>().ParticleType == "3Helium" && b.GetComponent<MoleculeType>().ParticleType == "3Helium")
                        {
                
                        }
                    }
                }
            }
        }
        
    }

    private void DestroyLableFollower(GameObject ObjToRemoveLabel)
    {
        // This codebase implements labels (e.g. +) as GameObjects following particles
        // This method removes the label follower that follows ObjToRemoveLabel
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

    private void EnableLableFollower(GameObject ObjToRemoveLabel)
    {
        // This codebase implements labels (e.g. +) as GameObjects following particles
        // This method removes the label follower that follows ObjToRemoveLabel
        Transform ChildrenList = GameObject.Find("Lable Canvas").transform;
        for (int x = 0; x < ChildrenList.childCount; x++)
        {
            Transform child = ChildrenList.GetChild(x);
            if (child.gameObject.GetComponent<ImageFollower>().sphereToFollow == ObjToRemoveLabel)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void AddLabel(GameObject ObjToAddLabel, int LabelIdx, bool UsingScale = false, Vector3 scale = new Vector3())
    { 
        if (LabelIdx < (GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs.Length))
        {
            GameObject Lable = MonoBehaviour.Instantiate(GameObject.Find("Lable Canvas").GetComponent<LableManager>().imagePrefabs[LabelIdx], Vector3.zero, Quaternion.identity);
            if (UsingScale)
            {
                Lable.transform.localScale = scale;
            }
            Lable.transform.SetParent(GameObject.Find("Lable Canvas").transform, false);
            Lable.AddComponent<ImageFollower>().sphereToFollow = ObjToAddLabel;
        }
    }

    private void ChangeColor(int MoleculeColorIdx)
    {
        // When certain molecules are formed, atoms must be colored in certain ways
        // Colors assigned based on ParticleType variable of MoleculeType component
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

    IEnumerator ResetNew3Helium()
    {
        // When Deuterium collides with a proton, and the He-3 isotope is formed, the process must restart
        // Restarts after a 2s delay
        yield return new WaitForSeconds(2f);
        if (!SecondRound)
        {
            SecondRound = true;
            for (int idx = 0; idx < 3; idx++)
            {
                CurrentObjects[idx].SetActive(false);
                DestroyLableFollower(CurrentObjects[idx]);
                CurrentlyInactive.Add(CurrentObjects[idx]);
            }

            CurrentObjects.Clear();

            for (int idx = 0; idx < 3; idx++)
            {
                Particle Proton = new Particle("Hydrogen " + (idx + 3), 2f, ICColor.Hydrogen, mass: 100000f, scale: 2f);
                GameObject ProtonGO = Proton.Spawn();
                ProtonGO.transform.position = StartPosition[idx % 3];
                ProtonGO.AddComponent<MoleculeType>();
                CurrentObjects.Add(ProtonGO);
            }
        }
        // Second round
        else
        {
            // simpler alternative to temporarily stop motion to position new particles
            float OldTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            for (int idx = 0; idx < 3; idx++)
            {
                CurrentObjects[idx].transform.position = StartPosition[1] + idx * new Vector3(1f, 0f);
                CurrentlyInactive[idx].transform.position = StartPosition[2] + idx * new Vector3(1f, 0f);
                CurrentlyInactive[idx].SetActive(true);
                EnableLableFollower(CurrentlyInactive[idx]);
                CurrentObjects.Add(CurrentlyInactive[idx]);
            }
            CurrentlyInactive.Clear();
            yield return new WaitForSeconds(2f);
            Time.timeScale = OldTimeScale;
        }
    }

    IEnumerator DeleteBetaNeutrino(GameObject Beta, GameObject Neutrino)
    {
        // Deleting GameObjects after a time interval is difficult -- associated labels must be removed
        yield return new WaitForSeconds(2f);
        DestroyLableFollower(Beta);
        Beta.SetActive(false);
        DestroyLableFollower(Neutrino);
        Neutrino.SetActive(false);
    }

    IEnumerator DeleteGamma(GameObject Gamma)
    {
        // Deleting GameObjects after a time interval is difficult -- associated labels must be removed
        yield return new WaitForSeconds(2f);
        DestroyLableFollower(Gamma);
        Gamma.SetActive(false);
    }
}
