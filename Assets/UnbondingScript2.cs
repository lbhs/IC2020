﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnbondingScript2 : MonoBehaviour
{
    private GameObject Atom1;      //Atom1 and Atom2 are the two atoms to unbond
    private GameObject Atom2;
    private int DotCount;       //This is used to make sure two dots are collided with--if only 1 dot, no bond to break!
    private GameObject Joule;    //the UI Jewel becomes a Colliding Joule to break bonds
    private Vector2 bondDirection;   //atoms unbond by moving Atom2 along the axis of the original bond
    private int MolIDValue;       //This variable gets the proper atom list from MoleculeList[].  Also used to push the atom list back in
    private GameObject SwapAtom;   //it is convenient to swap hydrogen to be Atom 2--this temporary variable allows for swapping Atom1 and Atom2
    private int JouleCost;     //the bond strength for the bond that is broken
    private int[,] bondArray;  //this array retrieves data from the master Bond Energy Array (attached to BondEnergyMatrix gameObject)
    FixedJoint2D jointToBreak;   //this variable allows examination of each bond (joint) in the molecule (one at a time)
    List<FixedJoint2D> jointsOnThisAtom;    //not using this--using the JointArray instead
    public FixedJoint2D[] JointArray = new FixedJoint2D[5];   //this stores the joints found on a given carbon atom (could be up to 4 joints)
    private int i; //used for indexing an array or list
    private int Index;  //used for assigning MoleculeID
    public int NewMoleculeID;  //used for a new carbon group that has been formed when C-C bond breaks
    private List<GameObject> TempAtomList;   //this list stores up the atoms that have broken away from the original molecule
    List<Rigidbody2D> BondingPartnerList;  //This is a Rigidbody list--when tracing bonds, need to use Rigidbody (joints connect RB's)
    private GameObject Diatomic;
    private Vector3 DiatomicPosition;
    private AudioSource SoundFX2;
    private GameObject JouleToDestroy;
    private GameObject[] JoulesInCorral;
    
    


    // Start is called before the first frame update
    void Start()
    {
        DotCount = 0;                //resets DotCount
        Joule = gameObject;
        JointArray = new FixedJoint2D[5];        
        TempAtomList = new List<GameObject>();
        BondingPartnerList = new List<Rigidbody2D>();
        SoundFX2 = GameObject.Find("BondBrokenSound").GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "UnbondingTrigger" || collider.tag == "UnbondingTriggerDB" || collider.tag == "Diatomic")

        {
            print("DotCount =" + DotCount);
            print("root object =" +collider.transform.root.gameObject);
            //DIATOMIC UNBONDING IS THE FIRST CASE
            if (collider.transform.root.gameObject.tag == "Diatomic")  //This section breaks a diatomic element into two atoms
            {
                print("dissociate diatomic!");
                Diatomic = collider.transform.root.gameObject;
                if (Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy > DisplayJoules.JouleTotal) 
                {
                    GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().Denied();
                    //prints "You don't have enough joules to break this bond";
                    return;
                }
                else
                { 
                    DisplayJoules.JouleTotal -= Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy;  //BDE is a public defined in DiatomicScript
                    DisplayJoules.BonusPointTotal -= 10;   //diatomic molecule = 10 bonus pts, need to subtract when molecule is destroyed
                    DiatomicPosition = Diatomic.transform.position;
                    Instantiate(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, DiatomicPosition, Quaternion.identity);  //Dissociation Produce is a public GameObject defined in DiatomicScript (H, Cl or O)
                    Instantiate(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(DiatomicPosition.x + 2.5f, DiatomicPosition.y, 0f), Quaternion.Euler(0f, 0f, 180f));
                    Destroy(Diatomic);  //the line above spawns the second atom rotated 180  degrees from the first
                    SoundFX2.Play();   //Bond breaking sound
                    JoulesInCorral = GameObject.FindGameObjectsWithTag("JouleInCorral");   //fill array with all the joules in the corral
                    for (i = 0; i < Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy; i++)     //for as many joules as it takes to break the bond
                    {
                        Destroy(JoulesInCorral[i]);
                    }
                    print("Case 1 (diatomic) complete");
                    return;  //DIATOMIC DISSOCIATION IS CASE 1 AND NEEDS NO OTHER SCRIPTS
                }

            }

            if (DotCount == 0)     //this is the first trigger
            {
                DotCount = 1;
                Atom1 = collider.transform.root.gameObject;
                print("Atom1 =" + Atom1);
            }

            else if (DotCount == 1)  //indicates we have a second trigger (two unbonding triggers)
            {
                Atom2 = collider.transform.root.gameObject;
                print("Atom2 = " + Atom2);

                if (Atom1.GetComponent<BondMaker>().Monovalent)  //If there is a Monovalent atom, put it in Atom2 slot--simplifies later case work 
                {
                    SwapAtom = Atom2;
                    Atom2 = Atom1;
                    Atom1 = SwapAtom;
                    print("Atom1 and Atom2 swapped");
                }

                if (Atom1 != Atom2)
                {
                    print(Atom1 + " " + Atom2 + " unbond");
                   
                    //THIS IS THE CALCULATE JOULE COST FUNCTION--ALL BOND BREAKING CASES 2, 3 & 4 USE THIS!
                    bondArray = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>().bondEnergyArray;
                    if (collider.tag == "UnbondingTriggerDB")  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
                    {
                        JouleCost = bondArray[Atom1.GetComponent<BondMaker>().bondArrayID + 3, Atom2.GetComponent<BondMaker>().bondArrayID + 3];
                    }
                    else
                    {
                        JouleCost = bondArray[Atom1.GetComponent<BondMaker>().bondArrayID, Atom2.GetComponent<BondMaker>().bondArrayID];
                    }

                    print("JouleCost =" + JouleCost);  //JouleCost comes from the bondArray[], which is a copy of the Master Array attached to BondEnergyMatrix GameObject
                    if (JouleCost > DisplayJoules.JouleTotal)
                    {
                        GameObject.Find("NotEnoughJoulesDisplay").GetComponent<ConversationTextDisplayScript>().Denied(); //prints "You don't have enough joules to break this bond";
                        return;
                    }

                    DisplayJoules.JouleTotal -= JouleCost;                  

                    JoulesInCorral = GameObject.FindGameObjectsWithTag("JouleInCorral");   //fill array with all the joules in the corral
                    for (i = 0; i < JouleCost; i++)     //for as many joules as it takes to break the bond
                    {
                        Destroy(JoulesInCorral[i]);
                    }
                    //THIS ENDS THE CALCULATE JOULE COST FUNCTION
                    

                    MolIDValue = Atom1.GetComponent<BondMaker>().MoleculeID;   //MolIDValue is used in this script to store MoleculeID

                    MCTokenSearch();  //THIS IS A FUNCTION:  If a MoleculeCompletionToken is in this list, remove it and decrement pt value

                    if (AtomInventory.MoleculeList[MolIDValue].Count == 2)  //THIS IS CASE 2--ONLY TWO ATOMS TO UNBOND
                    {
                        ClearMoleculeList();  //THIS FUNCTION SETS EACH ATOM TO UNBONDED-0 STATE AND REMOVES JOINT THAT BONDS THEM
                        MoveAtomsAndAdjustValleys();
                        print("CASE2 complete--gonna return now");
                        return;   //this seems to be working!!!  (April 26 11:45 am)
                    }


                    /*else if (Atom1.GetComponent<BondMaker>().Monovalent) //(Atom1.tag == "Hydrogen" || Atom1.tag == "Chlorine")
                    {
                        Destroy(Atom1.GetComponent<FixedJoint2D>());
                        Atom1.GetComponent<BondMaker>().MoleculeID = 0;   //If Atom1 is H or Cl, must now be a single, unbonded atom
                        AtomInventory.MoleculeList[MolIDValue].Remove(Atom1);  
                        Atom1.GetComponent<BondMaker>().bonded = false;
                    }*/  //DON'T NEED THIS BECAUSE ATOM2 IS MONOVALENT, AND IF ATOM1 IS MONOVALENT, CASE 2 APPLIES

                    else if (Atom2.GetComponent<BondMaker>().Monovalent)   //THIS IS CASE3--SPLITTING A MONOVALENT FROM A GROUP OF AT LEAST 2 ATOMS
                    {
                        print("CASE3 initiated--dissociate monovalent atom");
                        Destroy(Atom2.GetComponent<FixedJoint2D>());  //every monovalent has a fixed joint
                        Atom2.GetComponent<BondMaker>().MoleculeID = 0;    //Reset monovalent Atom2 MoleculeID to zero (unbonded state)
                        Atom2.GetComponent<BondMaker>().bonded = false;   //reset monovalent Atom2 to unbonded state
                        AtomInventory.MoleculeList[MolIDValue].Remove(Atom2);  //take the monovalent out of the MoleculeList
                        MoveAtomsAndAdjustValleys();   //FOR CASE3
                        print("CASE3 completed");
                        return;
                    }



                    else //No monovalent involved in this unbonding:  At least one cluster of atoms is involved bc CASE2 was not met                   
                    
                    {
                        print("CASE4:  carbon-carbon bond is now broken");
                        print(AtomInventory.MoleculeList[MolIDValue].Count + " AtomInvCount");

                      
                        if(AtomInventory.MoleculeList[MolIDValue].Count > 2)  //this means that at least one of the Atoms has bonding partners--do contact tracing!
                        {     
                            Rigidbody2D Atom1rb = Atom1.GetComponent<Rigidbody2D>();  //need Rigidbody for the joint scripting!  GameObject name doesn't work!!
                            Rigidbody2D Atom2rb = Atom2.GetComponent<Rigidbody2D>();   //Atom2rb is the Rigidbody equivalent of Atom2


                            if (Atom1.GetComponent<FixedJoint2D>())   //trying to find the joint that links the two carbons we are unbonding
                            {
                                JointArray = Atom1.GetComponents<FixedJoint2D>();   //this gets ALL the bonds between this carbon and neighboring carbons/oxygens
                                print("Atom 1 joints:");
                                
                                foreach (FixedJoint2D jointToBreak in JointArray)  //Search through all the joints on Atom1 to find the one to break!
                                {
                                    //print(jointToBreak.connectedBody);  //just for debugging purposes

                                    if (jointToBreak.connectedBody == Atom2rb)   //this is the connector between the two carbon atoms
                                    {
                                        jointToBreak.connectedBody = null;   //need to do this to avoid Atom1 joining BondingPartnerList--it remembered its ConnectedBody
                                        Destroy(jointToBreak);
                                        print("joint from Atom1 to Atom2 broken");
                                    }
                                }
                            }

                            if (Atom2.GetComponent<FixedJoint2D>() == null)  //PROBABLY DON'T NEED THIS
                            {
                                BondingPartnerList.Add(Atom2.GetComponent<Rigidbody2D>());  //start the list with Atom2
                            }

                            else        //if Atom2 has bonds, add ALL the attached atoms to BondingPartnerList
                            {
                                BondingPartnerList.Add(Atom2.GetComponent<Rigidbody2D>());  //start the list with Atom2
                                JointArray = Atom2.GetComponents<FixedJoint2D>();  //searching Atom2 for all BondingPartners

                                foreach (FixedJoint2D jointToBreak in JointArray)
                                {
                                    if (jointToBreak.connectedBody == Atom1rb)   //Need to destroy the joint between Atom2 and Atom1
                                    {
                                        Destroy(jointToBreak);
                                        print("joint from Atom2 to Atom1 broken");  
                                    }

                                    BondingPartnerList.Add(jointToBreak.connectedBody);  //BondingPartnerList collects the rb's at the ends of joints from Atom2 
                                    BondingPartnerList.Remove(Atom1rb);   //this is iterated--not a problem?
                                }
                            }

                            //ATOM2 WILL BE MOVED WITH ALL ITS CONNECTED ATOMS
                            foreach (Rigidbody2D atomRB in BondingPartnerList)   //this list is currently only carbons and oxygens--hydrogens added later
                            {
                                print("Atom2 BondingPartnerList contains " + atomRB);  //just a check to see if the list is complete
                            }

                            for (i = 1; i < 5; i++)  //iterate the search so that distant contacts will be included in the BondingPartnerList
                            {
                                foreach (GameObject atom in AtomInventory.MoleculeList[MolIDValue])  //This is to move the hydrogens attached to carbons
                                {
                                    //THIS IS IT--THIS SCRIPT NOW LOOKS AT EVERY JOINT ON A POLYVALENT ATOM!!!!
                                    JointArray = atom.GetComponents<FixedJoint2D>();  //this will get all the joints on "atom"

                                    foreach (FixedJoint2D joint in JointArray)  //look at the joints one at a time
                                    {
                                        if (BondingPartnerList.Contains(joint.connectedBody))  //Looks for atoms in MoleculeList[] who have joints that target atoms in BondingPartnerList
                                        {
                                            if (BondingPartnerList.Contains(atom.GetComponent<Rigidbody2D>()))
                                            {
                                                print(atom + " is NOT added to BPL bc it's already present");
                                            }
                                            else
                                            {
                                                print(atom + " was added to BPL");   //this will display the subset of atoms that will move with Atom2
                                                BondingPartnerList.Add(atom.GetComponent<Rigidbody2D>());  //this should complete the BondingPartnerList--all atoms to dissociate from the main molecule
                                                BondingPartnerList.Remove(Atom1rb);
                                            }

                                        }

                                    }
                                }
                            }

                            
                            //need an empty MoleculeID to store the dissociated Atom2 cluster  THIS IS A FUNCTION used only when unbonding Carbons/Oxygens
                            for (i = 1; i < 13; i++)   //Slots 1-12 in the array are used to store Molecules (atoms in the molecule)
                            {
                                if (AtomInventory.MoleculeList[i] == null || AtomInventory.MoleculeList[i].Count == 0)
                                {
                                    Index = i;      //Index finds the lowest empty MoleculeList slot
                                    break;          //to abort the loop after the first empty slot is found
                                }
                            }


                            NewMoleculeID = Index;                         //Index shows the empty slot to use for the new MoleculeID 
                            AtomInventory.MoleculeList[MolIDValue].Remove(Atom2);  //take Atom2 out of the original MoleculeID--Atom2 always moves


                            foreach (Rigidbody2D BP in BondingPartnerList)  //BondingPartnerList is a list of all the atoms to move
                            {
                                if (BondingPartnerList.Count == 1)   //if only a single atom being dissociated, set to UNBONDED state
                                {
                                    print("Atom2 reset to Zero");
                                    Atom2.GetComponent<BondMaker>().bonded = false;  //BP.GetComponent<BondMaker>().bonded = false;    //set to unbonded--allows rotation and SwapIt
                                    Atom2.GetComponent<BondMaker>().MoleculeID = 0; //BP.GetComponent<BondMaker>().MoleculeID = 0;    //set molecule ID to zero
                                    //break;
                                }
                                else  //TempAtomList is the list of all atoms to be moved as a cluster.  Each "BP" is an atom in BondingPartnerList
                                {
                                    TempAtomList.Add(BP.gameObject);     //translates RigidbodyList to GameObjectList
                                    AtomInventory.MoleculeList[MolIDValue].Remove(BP.gameObject);   //removes BondingPartners of Atom2 from original MoleculeID
                                    BP.gameObject.GetComponent<BondMaker>().MoleculeID = NewMoleculeID;  //redesignates atom with correct ID}
                                    AtomInventory.MoleculeList[NewMoleculeID] = TempAtomList;  //stores the atoms that have been moved in a new MoleculeID slot
                                }

                            }

                            if (AtomInventory.MoleculeList[MolIDValue].Count == 1)  //check to see if Atom1 is all alone after moving out the Atom2 cluster, if so, MoleculeID = zero
                            {
                                print("Atom1 gets reset to MolID zero");  //originally didn't work for O2 bc MCToken counted as a game object!!! 
                                Atom1.GetComponent<BondMaker>().bonded = false;
                                Atom1.GetComponent<BondMaker>().MoleculeID = 0;
                                AtomInventory.MoleculeList[MolIDValue].Clear();
                            }
                        }

                        MoveAtomsAndAdjustValleys();  //THIS IS FOR CASE4  

                    }

                }
            }
        }

    }

    private void MCTokenSearch()  //If a MoleculeCompletionToken is in this list, remove it and decrement pt value
    {
        foreach (GameObject MCToken in AtomInventory.MoleculeList[MolIDValue])
        {
            if (MCToken.tag == "MCToken")         //Unbonding removes the COMPLETED MOLECULE TOKEN from the MoleculeList 
            {
                print("MC Token Found and Removed");
                AtomInventory.MoleculeList[MolIDValue].Remove(MCToken);    //MC Token removed from list
                DisplayJoules.BonusPointTotal -= MCToken.GetComponent<MoleculePtValues>().BonusPtValue;  //Value of MCToken subtracted from score
                break;
            }
        }
    }

    private void ClearMoleculeList()
    {
        print("hello world");
        AtomInventory.MoleculeList[MolIDValue].Clear();   //Empty the molecule list b/c both atoms become unbonded
        print("cleared MoleculeList");
        Atom1.GetComponent<BondMaker>().MoleculeID = 0;  // Set MoleculeID of Atom1 to zero
        print("Atom1 set to zero");
        Atom2.GetComponent<BondMaker>().MoleculeID = 0;  //Set MoleculeID of atom2 to zero
        print("Atom2 set to zero");
        Atom1.GetComponent<BondMaker>().bonded = false;  //Set bonded state to false for both atoms
        Atom2.GetComponent<BondMaker>().bonded = false;
        if (Atom1.GetComponent<FixedJoint2D>())    //remove the bond!
        {
            Destroy(Atom1.GetComponent<FixedJoint2D>());
        }
        if (Atom2.GetComponent<FixedJoint2D>())
        {
            Destroy(Atom2.GetComponent<FixedJoint2D>());
        }
    }

    private void MoveAtomsAndAdjustValleys()
    {
        //the line of code below moves the unbonded atoms apart by a reasonable distance
        bondDirection = (Atom2.transform.position - Atom1.transform.position); //finds the vector that lines up the two atoms
        Atom2.transform.position = new Vector2(Atom2.transform.position.x + 0.4f * bondDirection.x, Atom2.transform.position.y + 0.4f * bondDirection.y);
        Atom1.GetComponent<BondMaker>().valleysRemaining++;   //an empty bonding slot has appeared on Atom1
        Atom2.GetComponent<BondMaker>().valleysRemaining++;    //an empty bonding slot has appeared on Atom2
        SoundFX2.Play();   //Plays Unbonding Sound
    }
    
    // Update is called once per frame
    void Update()
    {
        Atom1 = null;
        Atom2 = null;
        DotCount = 0;
        Destroy(Joule);
    }
}