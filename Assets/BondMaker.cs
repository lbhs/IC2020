using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondMaker: MonoBehaviour
{
    //This script makes a bond (fixed joint) when triggered by a radical that overlaps a "hole"

    FixedJoint2D joint;
    
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
    public static int Index = 1;  //Counts the number of molecules built
    public int MoleculeID;
    private List<GameObject> TempAtomList;
    private int BonusPts;
   
    
    


    // Start is called before the first frame update
    void Start()
    {
        bonded = false;
        TempAtomList = new List<GameObject>();
       
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
                                
                if (MoleculeID == 0  && BondingPartner.GetComponent<BondMaker>().MoleculeID == 0)  //New molecule has begun!
                {
                    MoleculeID = Index;   //Index will increment to assign an unused MoleculeID value (max MoleculeID = 12)
                    BondingPartner.GetComponent<BondMaker>().MoleculeID = Index;    
                    Index++;
                    print("New moleculeID = " + MoleculeID);     
                    TempAtomList.Add(gameObject);
                    AtomInventory.MoleculeList[MoleculeID]=TempAtomList;   //to avoid a null error message when trying to access an empty list
                }

                else if (MoleculeID == 0 && BondingPartner.GetComponent<BondMaker>().MoleculeID > 0)  //this means the Bonding Partner already has a MoleculeID
                {
                    //NEED TO MERGE LISTS HERE!!!!
                    MoleculeID = BondingPartner.GetComponent<BondMaker>().MoleculeID;  //newly bonded atom takes on the MoleculeID of its partner
                    print("Took on BP moleculeID = " + MoleculeID);   
                }

                else if (MoleculeID > 0 && BondingPartner.GetComponent<BondMaker>().MoleculeID == 0)    //if this bonding GameObject already has a MoleculeID (it was bonded earlier)
                {
                    //NEED TO MERGE LISTS HERE!!!!
                    BondingPartner.GetComponent<BondMaker>().MoleculeID = MoleculeID;  //BondingPartner takes on this atom's MoleculeID
                    print("BP moleculeID matches this atom= " + MoleculeID);
                }

                joint = gameObject.AddComponent<FixedJoint2D>();                 //joint links to centers of bonding atoms                                                               
                joint.connectedBody = BondingPartner.GetComponent<Rigidbody2D>(); //parent of the "Peak"
                joint.autoConfigureConnectedAnchor = false;              //if this bool is true, the joint won't hold when object is dragged!
                joint.enableCollision = false;                         //so no additional joints will be created (avoid infinite loop)

                TempAtomList = AtomInventory.MoleculeList[MoleculeID];  //TempAtomList gets the stored list from MoleculeList Array


                if (TempAtomList.Contains(gameObject))    
                {
                    print("already in list");
                }

                else
                {
                    TempAtomList.Add(gameObject);  //add this gameObject to the list that will be stored in MoleculeListKeeper
                    print("Added this to this molecule's list: " + gameObject);
                    AtomInventory.MoleculeList[MoleculeID] = TempAtomList;  //pushes the TempAtomList into this molecule's ListKeeper Slot
                    print("Molecule ID" + MoleculeID);
                    foreach (GameObject atom in AtomInventory.MoleculeList[MoleculeID])
                    {
                        print(atom.name);
                    }
                    
                }
               

                if (TempAtomList.Contains(BondingPartner))     //(GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().AtomList.Contains(BondingPartner))
                {
                    print("BP already in list");
                }

                else
                {
                    TempAtomList.Add(BondingPartner);
                    print("Added to list: " + BondingPartner);
                    AtomInventory.MoleculeList[MoleculeID] = TempAtomList;
                    print("MoleculeID =" +MoleculeID);
                    foreach (GameObject atom in AtomInventory.MoleculeList[MoleculeID]) //GameObject.Find("MoleculeListKeeper").GetComponent<//AtomInventory>().MoleculeList[Index])
                        {
                        print(atom.name);
                    }
                    
                    
                }
                

                //maintenance of bonding states and valley counts

                bonded = true;          //bonded state disables atom rotation
                valleysRemaining--;         //decrement number of bonding spots to fill on this atom
                collider.transform.root.gameObject.GetComponent<BondMaker>().valleysRemaining--;    //decrease bonding slots on BondingPartner
                collider.transform.root.gameObject.GetComponent<BondMaker>().bonded = true;        //set BondingPartner to bonded state
                
                
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


                //this next section counts the number of unfilled bonding slots ("valleys")
                for (i = 0; i < TempAtomList.Count; i++)   
                {
                    totalValleysRem += TempAtomList[i].GetComponent<BondMaker>().valleysRemaining;  //TempAtomList is the list of all atoms belonging to the molecule formed by this bonding event
                }

                //totalValleysRem = counts up the empty slots on all the atoms in the molecule   
                print("total valleys remaining =" + totalValleysRem);
                if(totalValleysRem == 0)
                { print("molecule complete!!!!");         //i indicates the number of atoms in the molecule
                    if (TempAtomList.Count>6)
                        { i = 6; }
                    BonusPts = GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().bonusPts[i];
                    print("point value of this molecule =" + BonusPts);     //+ GameObject.Find("MoleculeListKeeper").GetComponent<AtomInventory>().bonusPts[i]);
                    DisplayJoules.BonusPointTotal += BonusPts;
                }

            }


        }


        

        

     }



    // Update is called once per frame
    void FixedUpdate()
    {
            
        colliderCount = 0;   //resets a faulty bonding attempt--without this reset, the atom stays in the "activated" state and can trigger faulty bonding events in future
        totalValleysRem = 0;  //reset the open bonding slot count so that the next collision starts at zero

        
    }
}
