using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IC2020;
using System;
using UnityEngine.UI;
using UnityEditor;

public class NuclearSimulation : MonoBehaviour
{
    private List<Vector3> StartPosition = new List<Vector3>();
    // An object in the scene is stored in CurrentObjects
    public List<GameObject> CurrentObjects = new List<GameObject>();
    public List<GameObject> CurrentlyInactive = new List<GameObject>();
    bool SecondRound = false;    // the second round of producing Helium-3
    bool FinalCollisionRound = false;    // colliding two Helium-3 molecules in the final stage
    bool CollisionNotRegistered = true;   

    // Start is called before the first frame update
    void Start()
    {
        List<String> Names = new List<String>()
        {
            "Hydrogen",
            "Deuterium",
            "Helium-3",
            "Helium-4",
            "Beta Particle",
            "Neutrino"
        };

        List<Color> ParticleColors = new List<Color>()
        {
            ICColor.Hydrogen,
            ICColor.Deuterium,
            ICColor.HeliumIsotope,
            ICColor.HeliumIsotope4,
            ICColor.Electron,
            ICColor.Neutrino
        };

        GameObject BTPanel = GameObject.Find("Buffet Table").transform.GetChild(0).gameObject;

        for (int idx = 0; idx < BTPanel.transform.childCount; idx++)
        {
            GameObject BTElement = BTPanel.transform.GetChild(idx).gameObject;
            
            if (BTElement.name == "Tile" + (idx - 1))
            {
                GameObject Indicator = BTElement.transform.GetChild(0).gameObject;
                GameObject Name = BTElement.transform.GetChild(1).gameObject;

                Name.GetComponent<Text>().text = Names[idx - 1];

                Indicator.GetComponent<Image>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
                Indicator.GetComponent<Image>().color = ParticleColors[idx - 1];
                Indicator.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);

                GameObject.Destroy(Indicator.GetComponent<UIDragNDrop>());
            }
        }

        // Going back to the first design: fixing the starting positions of the protons
        StartPosition.Add(new Vector3(-7f, 7f));
        StartPosition.Add(new Vector3(0f, -7f));
        StartPosition.Add(new Vector3(7f, 7f));

        // Also the first solution: giving each proton an incredible mass
        for (int x = 0; x < 3; x++)
        {
            Particle Proton = new Particle("Hydrogen " + x, 2f, ICColor.Hydrogen, mass: 100000f, scale: 2f);
            GameObject ProtonGO = Proton.Spawn();
            ProtonGO.tag = "Hydrogen";
            ProtonGO.transform.position = StartPosition[x % 3];
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
                    // there is no way to detect a collision without adding a component to each GO
                    float ObjRadius = a.GetComponent<SphereCollider>().radius * a.transform.localScale.x;    // sphere must have identical scale in each direction
                    if (Math.Abs(Vector3.Distance(a.transform.position, b.transform.position) - 2 * ObjRadius) <= .25f)
                    {
                        // Scenario 1: Two protons, not part of any molecule, have collided
                        if (a.tag == "Hydrogen" && b.tag == "Hydrogen")
                        {
                            // Join the protons to form deuterium
                            a.AddComponent<FixedJoint>().connectedBody = b.GetComponent<Rigidbody>();
                            a.tag = "Deuterium";
                            b.tag = "Deuterium";
                            b.name = "[P] Neutron " + b.name.Split(' ')[2];    
                            b.GetComponent<charger>().charge = 0f;
                            DestroyLableFollower(b);
                            // '0' designates neutral charge
                            AddLabel(b, 3);
                            ChangeColor(0);

                            Particle Neutrino = new Particle("Neutrino", 0f, ICColor.Neutrino, mass: 0f);
                            GameObject NeutrinoGO = Neutrino.Spawn();
                            DestroyLableFollower(NeutrinoGO);
                            AddLabel(NeutrinoGO, 3);

                            Particle Beta = new Particle("Beta", 2f, ICColor.Electron, mass: 0f);
                            GameObject BetaGO = Beta.Spawn();

                            // the beta and neutrino cannot be deleted immediately after their creation
                            // the coroutine I created deletes them after 2s
                            StartCoroutine(DeleteBetaNeutrino(BetaGO, NeutrinoGO));
                        }

                        // Scenario 2: A particle in deuterium and a free proton have collided
                        else if (a.tag == "Deuterium" && b.tag == "Hydrogen")
                        {
                            a.tag = "3Helium";
                            b.tag = "3Helium";

                            a.AddComponent<FixedJoint>().connectedBody = b.GetComponent<Rigidbody>();

                            // all three particles are part of Helium-3
                            for (int idx = 0; idx < CurrentObjects.Count; idx++)
                            {
                                GameObject GO = CurrentObjects[idx];
                                if (GO != a && GO != b)
                                {
                                    GO.tag = "3Helium";
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
                        
                        // Final case: two Helium-3 isotopes have collided
                        else if (a.tag == "3Helium" && b.tag == "3Helium" && FinalCollisionRound && CollisionNotRegistered)
                        {
                            CollisionNotRegistered = false;

                            for (int idx = 0; idx < CurrentObjects.Count; idx++)
                            {
                                DestroyLableFollower(CurrentObjects[idx]);
                                CurrentObjects[idx].SetActive(false);
                            }

                            GameObject PrevObject = null;

                            for (int idx = 0; idx < 4; idx++)
                            {
                                Particle Hydrogen = new Particle("Hydrogen " + (idx + 6), 2f, ICColor.HeliumIsotope4, scale: 2f);
                                GameObject HydrogenGO = Hydrogen.Spawn();

                                // 2 protons, 2 neutrons in Helium-4
                                if (idx > 1)
                                {
                                    DestroyLableFollower(HydrogenGO);
                                    AddLabel(HydrogenGO, 3);
                                    HydrogenGO.name = "[P] Neutron " + HydrogenGO.name.Split(' ')[2];
                                }

                                if (!(PrevObject == null))
                                {
                                    HydrogenGO.AddComponent<FixedJoint>().connectedBody = PrevObject.GetComponent<Rigidbody>();
                                }

                                PrevObject = HydrogenGO;
                            }

                            Particle StdHydrogen = new Particle("Hydrogen 11", 2f, ICColor.Hydrogen, scale: 2f);
                            StdHydrogen.Spawn();

                            Particle StdHydrogen2 = new Particle("Hydrogen 12", 2f, ICColor.Hydrogen, scale: 2f);
                            StdHydrogen2.Spawn();
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
                GameObject.Destroy(child.gameObject);
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
            Lable.GetComponent<ImageFollower>().sphereToFollow = ObjToAddLabel;
        }
    }

    private void ChangeColor(int MoleculeColorIdx)
    {
        // When certain molecules are formed, atoms must be colored in certain ways
        // Colors assigned based on ParticleType variable of MoleculeType component
        for (int x = 0; x < CurrentObjects.Count; x++)
        {
            GameObject GO = CurrentObjects[x];
            if (GO.tag == "Deuterium" && MoleculeColorIdx == 0)
            {
                GO.GetComponent<Renderer>().material.color = ICColor.Deuterium;
            }

            else if (GO.tag == "3Helium" && MoleculeColorIdx == 1)
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
                ProtonGO.tag = "Hydrogen";
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
                // change position and status
                CurrentObjects[idx].transform.position = StartPosition[1] + idx * new Vector3(1f, 1f);
                CurrentlyInactive[idx].transform.position = StartPosition[2] - idx * new Vector3(1f, 1f);
                CurrentlyInactive[idx].SetActive(true);

                if (CurrentlyInactive[idx].name.Split(' ')[1] == "Hydrogen")
                {
                    AddLabel(CurrentlyInactive[idx], 0);
                }
                else
                {
                    AddLabel(CurrentlyInactive[idx], 3);
                }
                CurrentObjects.Add(CurrentlyInactive[idx]);
            }

            CurrentlyInactive.Clear();
            FinalCollisionRound = true;

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
        yield return new WaitForSeconds(2f);
        DestroyLableFollower(Gamma);
        Gamma.SetActive(false);
    }
}
