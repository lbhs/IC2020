using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondMaker: MonoBehaviour
{
    //This script makes a bond (fixed joint) when triggered by simultaneous collision of "peaks" and "valleys" Double Handshake

    FixedJoint2D joint;
    FixedJoint2D joint1;
    FixedJoint2D joint2;
    
    public bool bonded;     //when bonded, atom no longer rotates
    public int colliderCount;  //the trigger value--need double handshake for a bond to form
    private int otherColliderCount;  //trigger on the other collider--completes double handshake
    public int valleysRemaining;  //number of bonding slots to fill:  H = 1, O = 2, N = 3, C = 4, etc.
    private int totalValleysRem;  //calculated each bonding event--when reaches zero, molecule is complete! All bonding slots filled
    public int bondArrayID;  //H = 0, C = 1, O = 2, Cl = 3, C with double bond = 4, O with double bond = 5 
    private int[,] bondArray;  //temporary storage of the bondEnergyArray stored in game object BondEnergyMatrix
    private int BondEnergy;  //value in joules of the bond that has just been formed
    private GameObject BondingPartner;  //the atom to which this atom has bonded
    private int i;  //counting integer in "for" loop
    public static int Index = 1;  //temporary variable used to assign Molecule ID values
    public int MoleculeID;          
    private int BondingPartnerMoleculeID;
    private List<GameObject> TempAtomList;       //used to make hydrogens always go in Atom2 slot (unless it's H2)
    private int BonusPts;           //Bonus Pt value for completed molecule
    private GameObject MCToken;
    public AudioSource SoundFX;     //Bond Formed Sound
    private AudioSource SoundFX3;   //Molecule Completion sound
    public GameObject Badge;
    public bool Monovalent;
    private GameObject AtomHere;
    public GameObject BadgeRecipient;

          


    // Start is called before the first frame update
    void Start()
    {
        bonded = false;
        TempAtomList = new List<GameObject>();
        SoundFX = GameObject.Find("BondMadeSound").GetComponent<AudioSource>();
        SoundFX3 = GameObject.Find("MoleculeCompleteSound").GetComponent<AudioSource>();

    }
    void OnTriggerEnter2D(Collider2D collider)  //triggered by an object colliding with a "Valley"
    {
       
        if(collider.tag == "Peak" || collider.tag == "PeakDB")     //only "Peaks" can make bonds with "Valleys"
        {
            colliderCount = 1;  //this marker indicates that this atom has received a bonding trigger, but need confirmation from the other atom prior to bond formation
            otherColliderCount = collider.transform.root.gameObject.GetComponent<BondMaker>().colliderCount;

            if(otherColliderCount == 1)  //this means that the other atom has triggered simultaneously = requirement for bond formation
             {
                BondingPartner = collider.transform.root.gameObject;  //add bonding partner to array of molecule's atoms
                BondingPartnerMoleculeID = BondingPartner.GetComponent<BondMaker>().MoleculeID;   //see what the Bonding partner's MoleculeID is
                //AtomHere = gameObject;
                
                if (MoleculeID == 0  && BondingPartnerMoleculeID == 0)  //New molecule has begun!  Only occurs when both MoleculeID = zero
                {
                    print("0-0, started a new molecule");
                    for (i=1;i<13;i++)   //Slots 1-12 in the array are used to store Molecules (atoms in the molecule)
                    {
                        if (AtomInventory.MoleculeList[i]== null || AtomInventory.MoleculeList[i].Count==0)
                        {
                            Index = i;      //Index finds the lowest empty MoleculeList slot
                            break;          //to abort the loop after the first empty slot is found
                        }
                    }
                    MoleculeID = Index;   //Index is used to assign an unused MoleculeID value (max MoleculeID = 12)
                    BondingPartner.GetComponent<BondMaker>().MoleculeID = Index;    //assign the same MoleculeID to both bonding atoms
                    TempAtomList.Add(gameObject);         //added one atom to the new MoleculeList
                    AtomInventory.MoleculeList[MoleculeID]=TempAtomList;   //push this new list to MoleculeList array to avoid a null error message when trying to access an empty list
                }

                else if (MoleculeID == 0 && BondingPartnerMoleculeID > 0)  //this means the Bonding Partner already has a MoleculeID
                {
                    print("gameObject" + gameObject + " took on Bonding Partner ID");
                    MoleculeID = BondingPartnerMoleculeID;  //newly bonded atom takes on the MoleculeID of its partner
                }

                else if (MoleculeID > 0 && BondingPartnerMoleculeID == 0)    //if this bonding GameObject already has a MoleculeID (it was bonded earlier)
                {
                    print("Bonding Partner took on ID of" + gameObject);
                    BondingPartner.GetComponent<BondMaker>().MoleculeID = MoleculeID;  //BondingPartner takes on this atom's MoleculeID
                }

                else //if (MoleculeID > 0 && BondingPartnerMoleculeID > 0)  //&& MoleculeID != BondingPartnerMoleculeID
                {
                    print("Merging Molecule Lists");
                    print("MoleculeID of this gameObject");
                    print("BondingPartnerMoleculeID =" + BondingPartnerMoleculeID);
                    //Merging lists--all atoms take on MoleculeID of this Molecule--then BondingPartnerMoleculeID is emptied
                    foreach (GameObject atom in AtomInventory.MoleculeList[BondingPartnerMoleculeID])  
                    {
                        AtomInventory.MoleculeList[MoleculeID].Add(atom);          //add each atom in Bonding Partner List to this MoleculeList[ID]
                        atom.GetComponent<BondMaker>().MoleculeID = MoleculeID;              //change the MoleculeID of each atom that is moved to new list
                    }
                    print("emptied Bonding Partner Molecule List");
                    AtomInventory.MoleculeList[BondingPartnerMoleculeID].Clear();  //makes the list empty, but not "null"
                }

                if (gameObject.tag == "Hydrogen" || gameObject.tag == "Chlorine")          //joints preferentially localized on hydrogen/Cl atoms--easier to unbond
                {
                    print("Joint added to Monovalent atom");     //Monovalent atoms get only one joint
                    joint = gameObject.AddComponent<FixedJoint2D>();                 //joint links to centers of bonding atoms                                                               
                    joint.connectedBody = BondingPartner.GetComponent<Rigidbody2D>();     //parent of the "Peak"
                    joint.autoConfigureConnectedAnchor = false;              //if this bool is true, the joint won't hold when object is dragged!
                    joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
                }
                else if (BondingPartner.GetComponent<BondMaker>().Monovalent)
                {
                    print("Joint added to monovalent Bonding Partner");                //the other atom might be monovalent--just one bond formed
                    joint = BondingPartner.AddComponent<FixedJoint2D>();            //BondingPartner could be H, O, C, Cl. . .
                    joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                    joint.autoConfigureConnectedAnchor = false;              //if this bool is true, the joint won't hold when object is dragged!
                    joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
                }

                else if (!gameObject.GetComponent<BondMaker>().Monovalent && !BondingPartner.GetComponent<BondMaker>().Monovalent)    //if neither atom is Monovalent, add joints to both atoms!
                {
                    print("Joint added to Bonding Partner (not monovalent)");
                    joint1 = BondingPartner.AddComponent<FixedJoint2D>();            //BondingPartner can only be O or C 
                    joint1.connectedBody = gameObject.GetComponent<Rigidbody2D>();    //PERHAPS THIS CAN BE USED TO TRACE CONTACTS. . .
                    joint2 = gameObject.AddComponent<FixedJoint2D>();
                    joint2.connectedBody = BondingPartner.GetComponent<Rigidbody2D>();  //joints on both bonding atoms!!
                    joint1.autoConfigureConnectedAnchor = false;              //if this bool is true, the joint won't hold when object is dragged!
                    joint1.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
                    joint2.autoConfigureConnectedAnchor = false;
                    joint2.enableCollision = false;
                }

                /*
                joint1.autoConfigureConnectedAnchor = false;              //if this bool is true, the joint won't hold when object is dragged!
                joint1.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)
                joint2.autoConfigureConnectedAnchor = false;
                joint2.enableCollision = false;
                */

                TempAtomList = AtomInventory.MoleculeList[MoleculeID];      //TempAtomList gets the stored list from MoleculeList Array


                if (TempAtomList.Contains(gameObject))    
                {
                    print(gameObject + " already in list");  //avoid duplication of GameObjects in the list
                }

                else
                {
                    print("Added to TempAtomList" + gameObject);
                    TempAtomList.Add(gameObject);  //add this gameObject to the list that will be stored in MoleculeList[] under MoleculeListKeeper
                    AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //pushes the TempAtomList into this molecule's ListKeeper Slot
                }
               

                if (TempAtomList.Contains(BondingPartner))   
                { 
                    print("BP already in list");  //no duplication of atoms desired!
                }

                else
                {
                    print("Added " +BondingPartner + "to TempAtomList");
                    TempAtomList.Add(BondingPartner);       //add BondingPartner to Atom List for storage in MoleculeList[]  
                    AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //TempAtomList pushed to MoleculeList[] array
                }
                

                //maintenance of bonding states and valley counts

                bonded = true;          //bonded state disables atom rotation
                valleysRemaining--;         //decrement number of bonding spots to fill on this atom
                collider.transform.root.gameObject.GetComponent<BondMaker>().valleysRemaining--;    //decrease bonding slots on BondingPartner
                collider.transform.root.gameObject.GetComponent<BondMaker>().bonded = true;        //set BondingPartner to bonded state
                SoundFX.Play();

               
                
                //this section of code finds the Bond Energy value in the 2D bondArray --needs identity of the two atoms making the bond (order is irrelevant)

                bondArray = GameObject.Find("BondEnergyMatrix").GetComponent<BondEnergyValues>().bondEnergyArray; //accesses the array of Bond Energies

                if (collider.tag == "PeakDB")  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
                {
                    BondEnergy = bondArray[gameObject.GetComponent<BondMaker>().bondArrayID+3, collider.transform.root.gameObject.GetComponent<BondMaker>().bondArrayID+3];
                }
                else
                {
                    BondEnergy = bondArray[gameObject.GetComponent<BondMaker>().bondArrayID, collider.transform.root.gameObject.GetComponent<BondMaker>().bondArrayID];
                }

                print("BondEnergy =" +BondEnergy);  
                DisplayJoules.JouleTotal += BondEnergy;        //this updates the total joule count from all bonds that the player has formed so far

                for (i = 0; i < BondEnergy; i++)  //adds the right number of joules to the JouleCorral
                {
                    GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().JSpawn();
                }

                //this next section counts the number of unfilled bonding slots ("valleys")
                print("TempAtomList atom count" + TempAtomList.Count);
                for (i = 0; i < TempAtomList.Count; i++)   
                {
                    print(TempAtomList[i]);
                    totalValleysRem += TempAtomList[i].GetComponent<BondMaker>().valleysRemaining;  //TempAtomList is the list of all atoms belonging to the molecule formed by this bonding event
                }

                //totalValleysRem = counts up the empty slots on all the atoms in the molecule   
                print("total valleys remaining =" + totalValleysRem);
                if(totalValleysRem == 0)
                {
                    print("molecule complete!!!!");         //i indicates the number of atoms in the molecule
                    SoundFX3.Play();
                    if (TempAtomList.Count>6)         //BonusPts max out at 6-atoms in a molecule
                        { i = 6; }   
                    BonusPts = GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().bonusPts[i];  //access the BonusPt Array
                    print("point value of this molecule =" + BonusPts);     //+ GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().bonusPts[i]);
                    DisplayJoules.BonusPointTotal += BonusPts;          //update BonusPointTotal static variable
                    MCToken = GameObject.Find("MoleculeListKeeper").GetComponent<MoleculeCompletionPtArray>().MoleculeCompletionToken[i];
                    AtomInventory.MoleculeList[MoleculeID].Add(MCToken);  //adds a MoleculeCompletionToken to the MoleculeList Array
                    //HERE'S THE NEW STUFF!!!
                    /*print("i =" + i);
                    if (i > 2)
                    {
                        foreach (GameObject atom in TempAtomList)
                        {
                            if (atom.tag == "Carbon")
                            {
                                BadgeRecipient = atom;
                                print("BadgeRecipient =" + BadgeRecipient);
                                break;
                            }
                            else if (atom.tag == "Oxygen")
                            {
                                BadgeRecipient = atom;
                                print("BadgeRecipient =" + BadgeRecipient);
                                break;
                            }

                        }

                        print("applying badge now");
                        GameObject.Find("Badge").GetComponent<ImageFollower>().objectToFollow = BadgeRecipient;
                        GameObject.Find("Badge").GetComponent<ImageFollower>().AddBadge();
                    }  //END OF NEW STUFF*/
                }
            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        colliderCount = 0;   //resets a faulty bonding attempt--without this reset, the atom stays in the "activated" state and can trigger faulty bonding events in future
        otherColliderCount = 0;
        totalValleysRem = 0;  //reset the open bonding slot count so that the next collision starts at zero
    }
}
