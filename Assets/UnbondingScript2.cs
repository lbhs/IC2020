using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbondingScript2 : MonoBehaviour
{
    public GameObject Atom1;      //Atom1 and Atom2 are the two atoms to unbond
    public GameObject Atom2;
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
        print(collider.transform.root.gameObject);
        if(collider.transform.root.gameObject.tag == "Diatomic")
        {
            print("dissociate diatomic!");
            Diatomic = collider.transform.root.gameObject;
            if(Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy > DisplayJoules.JouleTotal)
            {
                print("not enough joules to break this bond");
                return;
            }
            DisplayJoules.JouleTotal -= Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy;
            DiatomicPosition = Diatomic.transform.position;
            Instantiate(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, DiatomicPosition, Quaternion.identity);  //makes a single atom of H, Cl or O
            Instantiate(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(DiatomicPosition.x + 2.5f, DiatomicPosition.y, 0f), Quaternion.Euler(0f, 0f, 180f));
            // Destroy(Diatomic);  //the line above spawns the second atom rotated 180  degrees from the first
            SoundFX2.Play();
            JoulesInCorral = GameObject.FindGameObjectsWithTag("JouleInCorral");   //fill array with all the joules in the corral
            for (i = 0; i < Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy; i++)     //for as many joules as it takes to break the bond
            {
                Destroy(JoulesInCorral[i]);
            }

            Destroy(Diatomic);
        }

        else if (DotCount == 0)
        {
            DotCount = 1;
            Atom1 = collider.transform.root.gameObject;
            print("Atom1 =" + Atom1);
        }

        else if (DotCount == 1)
        {
            Atom2 = collider.transform.root.gameObject;
            print("Atom2 = " + Atom2);

            if (Atom1.tag == "Hydrogen")
            {
                SwapAtom = Atom2;
                Atom2 = Atom1;
                Atom1 = SwapAtom;
            }

            if (Atom1 != Atom2)
            {
                print(Atom1 + " " + Atom2 + " unbond");
                print("collider = " +collider.tag);
                //CALCULATE JOULE COST HERE--IF NOT ENOUGH JOULES, ABORT THIS MISSION!
                bondArray = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>().bondEnergyArray;
                if (collider.tag == "UnbondingTriggerDB")  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
                {
                    JouleCost = bondArray[Atom1.GetComponent<BondMaker>().bondArrayID + 3, Atom2.GetComponent<BondMaker>().bondArrayID + 3];
                }
                else
                {
                    JouleCost = bondArray[Atom1.GetComponent<BondMaker>().bondArrayID, Atom2.GetComponent<BondMaker>().bondArrayID];
                }

                print("JouleCost =" + JouleCost);    
                if(JouleCost > DisplayJoules.JouleTotal)
                {
                    print("can't break this bond!");    //NEED TO PRINT THIS ON SCREEN SO PLAYERS CAN SEE!
                    return;
                }

                DisplayJoules.JouleTotal -= JouleCost;

                JoulesInCorral = GameObject.FindGameObjectsWithTag("JouleInCorral");   //fill array with all the joules in the corral
                for (i = 0; i < JouleCost; i++)     //for as many joules as it takes to break the bond
                {
                    Destroy(JoulesInCorral[i]);
                }
                    


                MolIDValue = Atom1.GetComponent<BondMaker>().MoleculeID;
                bondDirection = (Atom2.transform.position - Atom1.transform.position); //finds the vector that lines up the two atoms
                
                if (Atom1.tag == "Hydrogen" || Atom1.tag == "Chlorine")
                {
                    Destroy(Atom1.GetComponent<FixedJoint2D>());
                    Atom1.GetComponent<BondMaker>().MoleculeID = 0;
                    AtomInventory.MoleculeList[MolIDValue].Remove(Atom1);
                    Atom1.GetComponent<BondMaker>().bonded = false;
                                       
                }
                if (Atom2.tag == "Hydrogen" || Atom1.tag == "Chlorine")
                {
                    Destroy(Atom2.GetComponent<FixedJoint2D>());
                    Atom2.GetComponent<BondMaker>().MoleculeID = 0;
                    AtomInventory.MoleculeList[MolIDValue].Remove(Atom2);
                    Atom2.GetComponent<BondMaker>().bonded = false;
                    //NEED TO MAKE ATOM1 BONDED = FALSE IF IT IS AN OXYGEN OR CARBON THAT IS ALL ALONE
                    if (AtomInventory.MoleculeList[MolIDValue].Count == 1)  //check to see if Atom1 is all alone, if so, MoleculeID = zero
                    {
                        Atom1.GetComponent<BondMaker>().bonded = false;
                        Atom1.GetComponent<BondMaker>().MoleculeID = 0;
                        AtomInventory.MoleculeList[MolIDValue].Clear();
                    }
                }



                if (Atom2.tag != "Hydrogen" && Atom1.tag != "Hydrogen")  //Unbonding Carbons is Complicated!!!!
                {
                    print("carbon-carbon bond");
                    Rigidbody2D Atom1rb = Atom1.GetComponent<Rigidbody2D>();  //need Rigidbody for the joint scripting!  GameObject name doesn't work!!
                    Rigidbody2D Atom2rb = Atom2.GetComponent<Rigidbody2D>();  
                    

                    if (Atom1.GetComponent<FixedJoint2D>())   //trying to find the joint that links the two carbons we are unbonding
                    {
                        JointArray = Atom1.GetComponents<FixedJoint2D>();   //this gets all the bonds between this carbon and neighboring carbons/oxygens
                        print("Atom 1 joints:");
                        foreach (FixedJoint2D jointToBreak in JointArray) 
                        {
                            print(jointToBreak.connectedBody);
                            
                            if(jointToBreak.connectedBody == Atom2rb)   //this is the connector between the two carbon atoms
                            {
                                jointToBreak.connectedBody = null;   //need to do this to avoid Atom1 joining BondingPartnerList--it remembered its ConnectedBody
                                Destroy(jointToBreak);
                                print("joint on Atom1 broken");
                                
                            }
                        }
                    }

                    if (Atom2.GetComponent<FixedJoint2D>() == null)
                    {
                        BondingPartnerList.Add(Atom2.GetComponent<Rigidbody2D>());  //start the list with Atom2
                        
                    }
                    else                //if Atom2 has bonds, add the attached atoms to BondingPartnerList
                    {
                        BondingPartnerList.Add(Atom2.GetComponent<Rigidbody2D>());  //start the list with Atom2
                        JointArray = Atom2.GetComponents<FixedJoint2D>();  //searching Atom2 for all BondingPartners. . .COULD MAKE THIS MORE EFFICIENT?

                        foreach (FixedJoint2D jointToBreak in JointArray)
                        {
                            if (jointToBreak.connectedBody == Atom1rb)
                            {
                                Destroy(jointToBreak);
                                print("joint on Atom2 broken");
                            }

                            BondingPartnerList.Add(jointToBreak.connectedBody);  //BondingPartnerList collects the rb's at the ends of joints from Atom2 
                            BondingPartnerList.Remove(Atom1rb);   //this is iterated--not a problem?

                        }
                    }
                    //SET BONDED TO FALSE IF BONDING PARTNER LIST HAS ONLY ONE ATOM IN IT!!!!!
                    foreach (Rigidbody2D atomRB in BondingPartnerList)   //this list is only carbons and oxygens--hydrogens added later
                    {
                            print("Atom2 BondingPartnerList contains " + atomRB);  //just a check to see if the list is complete

                    }  

                    foreach (GameObject atom in AtomInventory.MoleculeList[MolIDValue])  //This is to move the hydrogens attached to carbons
                    {
                        if (atom.GetComponent<FixedJoint2D>())               //every hydrogen has a FixedJoint2D
                        {
                            print("this atom has a FixedJoint" + atom);   //DELETE THIS
                            if (BondingPartnerList.Contains(atom.GetComponent<FixedJoint2D>().connectedBody))  //Looks for atoms in MOleculeList[] who have joints that target atoms in BondingPartnerList
                            {
                                BondingPartnerList.Add(atom.GetComponent<Rigidbody2D>());
                                print("This atom added to BPL:" + atom);   //DELETE THIS
                                print("connectedBody" + atom.GetComponent<FixedJoint2D>().connectedBody);  //DELETE THIS
                            }
                        }
                    }

                        

                    //need an empty MoleculeID to store the dissociated Atom2 cluster
                    for (i = 1; i < 13; i++)   //Slots 1-12 in the array are used to store Molecules (atoms in the molecule)
                    {
                        if (AtomInventory.MoleculeList[i] == null || AtomInventory.MoleculeList[i].Count == 0)
                        {
                            Index = i;      //Index finds the lowest empty MoleculeList slot
                            break;          //to abort the loop after the first empty slot is found
                        }
                    }


                    NewMoleculeID = Index;                         //Index shows the empty slot to use for the new MoleculeID 
                    AtomInventory.MoleculeList[MolIDValue].Remove(Atom2);  //take Atom2 out of the original MoleculeID
                    

                    foreach (Rigidbody2D BP in BondingPartnerList)  //BondingPartnerList is a list of all the atoms to move
                    {
                        if(BondingPartnerList.Count == 1)   //only a single atom being dissociated
                        {
                            BP.GetComponent<BondMaker>().bonded = false;    //set to unbonded--allows rotation and SwapIt
                            BP.GetComponent<BondMaker>().MoleculeID = 0;    //set molecule ID to zero
                            break;
                        }
                    TempAtomList.Add(BP.gameObject);     //translates RigidbodyList to GameObjectList
                    AtomInventory.MoleculeList[MolIDValue].Remove(BP.gameObject);   //removes BondingPartners of Atom2 from original MoleculeID
                    BP.gameObject.GetComponent<BondMaker>().MoleculeID = NewMoleculeID;  //redesignates atom with correct ID
                    
                    }
                    AtomInventory.MoleculeList[NewMoleculeID] = TempAtomList;  //stores the atoms that have been moved in a new MoleculeID slot

                    if (AtomInventory.MoleculeList[MolIDValue].Count == 1)  //check to see if Atom1 is all alone, if so, MoleculeID = zero
                    {
                        Atom1.GetComponent<BondMaker>().bonded = false;
                        Atom1.GetComponent<BondMaker>().MoleculeID = 0;
                        AtomInventory.MoleculeList[MolIDValue].Clear();
                    }
                }
                                
            }


                //the line of code below moves the unbonded atoms apart by a reasonable distance
                Atom2.transform.position = new Vector2(Atom2.transform.position.x + 0.4f*bondDirection.x, Atom2.transform.position.y + 0.4f*bondDirection.y);
                Atom1.GetComponent<BondMaker>().valleysRemaining++;   //an empty bonding slot has appeared on Atom1
                Atom2.GetComponent<BondMaker>().valleysRemaining++;    //an empty bonding slot has appeared on Atom2
                SoundFX2.Play();   //Plays Unbonding Sound

            if (AtomInventory.MoleculeList[MolIDValue] != null)
            {
                foreach (GameObject MCToken in AtomInventory.MoleculeList[MolIDValue])
                {
                    if (MCToken.tag == "MCToken")         //Unbonding removes the COMPLETED MOLECULE TOKEN from the MoleculeList 
                    {
                        print("this list contains an MC Token");
                        AtomInventory.MoleculeList[MolIDValue].Remove(MCToken);    //MC Token removed from list
                        DisplayJoules.BonusPointTotal -= MCToken.GetComponent<MoleculePtValues>().BonusPtValue;  //Value of MCToken subtracted from score
                        break;
                    }
                }
            }                                                             
        }
        

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
