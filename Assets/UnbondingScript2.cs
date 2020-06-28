using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    public FixedJoint2D[] JointArray = new FixedJoint2D[5];   //this stores the joints found on a given carbon atom (could be up to 4 joints)
    private int i; //used for indexing an array or list
    private int Index;  //used for assigning MoleculeID
    public int NewMoleculeID;  //used for a new carbon group that has been formed when C-C bond breaks
    private List<GameObject> TempAtomList;   //this list stores up the atoms that have broken away from the original molecule
    List<Rigidbody2D> BondingPartnerList;  //This is a Rigidbody list--when tracing bonds, need to use Rigidbody (joints connect RB's)
    private GameObject Diatomic;
    private Vector3 DiatomicPosition;
    private AudioSource SoundFX2;
    public static int DontBondAgain;
    public static int WaitABit;
    private int FrameActiveCount;
    private int CheckIndex;
    private PhotonView PV;
    private ComponentData CD;

    // Start is called before the first frame update
    void Start()
    {
        CD = GameObject.Find("ComponentReferences").GetComponent<ComponentData>();

        DotCount = 0;                //resets DotCount
        Joule = gameObject;
        JointArray = new FixedJoint2D[5];
        TempAtomList = new List<GameObject>();
        BondingPartnerList = new List<Rigidbody2D>();
        FrameActiveCount = 0;

        if (GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[0])
        {
            CheckIndex = 0;
        }
        else
        {
            CheckIndex = 1;
        }

        PV = GetComponent<PhotonView>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.tag == "UnbondingTrigger" || collider.tag == "UnbondingTriggerDB" || collider.tag == "Diatomic") 
            && GetComponent<PhotonView>().IsMine)
        {
            print("DotCount =" + DotCount);
            print("root object =" + collider.transform.root.gameObject);
            //DIATOMIC UNBONDING IS THE FIRST CASE
            if (collider.tag == "Diatomic")  //This section breaks a diatomic element into two atoms
            {
                print("dissociate diatomic!");
                Diatomic = collider.transform.root.gameObject;
                if (Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy > CD.JDC.TotalJoulesDisplaying[CheckIndex])
                {
                    Debug.Log("You don't have enough joules to break this bond!");
                    return;
                }
                else
                {
                    CD.JDC.GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, -Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy, Diatomic.GetComponent<PhotonView>().ViewID, -10);
                    DiatomicPosition = Diatomic.transform.position;
                    CD.GSC.NetowrkSpawn(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(DiatomicPosition.x - 1.0f, DiatomicPosition.y));
                    CD.GSC.NetowrkSpawn(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(DiatomicPosition.x + 1.2f, DiatomicPosition.y));
                    PhotonNetwork.Destroy(Diatomic);
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

                if (Atom1.GetComponent<AtomController>().Monovalent)  //If there is a Monovalent atom, put it in Atom2 slot--simplifies later case work 
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
                        JouleCost = bondArray[Atom1.GetComponent<AtomController>().EnergyMatrixPosition + 3, Atom2.GetComponent<AtomController>().EnergyMatrixPosition + 3];
                    }
                    else
                    {
                        JouleCost = bondArray[Atom1.GetComponent<AtomController>().EnergyMatrixPosition, Atom2.GetComponent<AtomController>().EnergyMatrixPosition];
                    }

                    print("JouleCost =" + JouleCost);  //JouleCost comes from the bondArray[], which is a copy of the Master Array attached to BondEnergyMatrix GameObject
                    if (JouleCost > CD.JDC.TotalJoulesDisplaying[CheckIndex])
                    {
                        Debug.Log("You don't have enough joules to break this bond!");
                        return;
                    }

                    MolIDValue = Atom1.GetComponent<AtomController>().MoleculeID;

                    // It doesn't matter whether we pass the PVID of Atom 1 or 2 -- they both have the same owner 
                    CD.JDC.GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, -JouleCost, Atom1.GetComponent<AtomController>().PVID, -CD.GSC.ReturnCompletionScore(MolIDValue));

                    if (CD.GSC.NumElementsInMolecule(MolIDValue) == 2)
                    {
                        Debug.Log("Unbond two elements!");
                        CD.GameSetupPhotonView.RPC("RemoveGivenElements", RpcTarget.All, MolIDValue);
                        PV.RPC("MoveAtomsAndAdjustValleys", RpcTarget.All, collider.tag, Atom1.GetComponent<AtomController>().PVID, Atom2.GetComponent<AtomController>().PVID);
                        if (Atom1.GetComponent<FixedJoint2D>())    //remove the bond!
                        {
                            PV.RPC("DeleteJointOverNetwork", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID);
                        }
                        if (Atom2.GetComponent<FixedJoint2D>())
                        {
                            PV.RPC("DeleteJointOverNetwork", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID);
                        }
                        PV.RPC("ChangeBondingState", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID, false);
                        PV.RPC("ChangeBondingState", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID, false);
                        return;
                    }

                    else if (Atom2.GetComponent<AtomController>().Monovalent)
                    {
                        Debug.Log("Atom2 is a monovalent!");
                        PV.RPC("DeleteJointOverNetwork", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID);
                        PV.RPC("ChangeBondingState", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID, false);
                        CD.GameSetupPhotonView.RPC("RemoveSetOfElements", RpcTarget.All, new int[1] { Atom2.GetComponent<AtomController>().PVID });
                        PV.RPC("MoveAtomsAndAdjustValleys", RpcTarget.All, collider.tag, Atom1.GetComponent<AtomController>().PVID, Atom2.GetComponent<AtomController>().PVID);
                        return;
                    }

                    else
                    {
                        if (CD.GSC.NumElementsInMolecule(MolIDValue) > 2)
                        {
                            Debug.Log("Complex unbonding!");
                            Rigidbody2D Atom1rb = Atom1.GetComponent<Rigidbody2D>();
                            Rigidbody2D Atom2rb = Atom2.GetComponent<Rigidbody2D>();

                            if (Atom1.GetComponent<FixedJoint2D>() != null)
                            {
                                JointArray = Atom1.GetComponents<FixedJoint2D>();

                                for (int i = 0; i < JointArray.Length; i++)
                                {
                                    if (JointArray[i].connectedBody == Atom2rb)
                                    {
                                        Debug.Log(JointArray[i].gameObject.name + " is attached to " + Atom2rb.gameObject.name);
                                        PV.RPC("DeleteJointOverNetworkByIndex", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID, i);
                                    }
                                }
                            }

                            BondingPartnerList.Add(Atom2.GetComponent<Rigidbody2D>());

                            if (Atom2.GetComponent<FixedJoint2D>() != null)
                            {
                                JointArray = Atom2.GetComponents<FixedJoint2D>();

                                for (int i = 0; i < JointArray.Length; i++)
                                {
                                    BondingPartnerList.Add(JointArray[i].connectedBody);

                                    if (JointArray[i].connectedBody == Atom1rb)
                                    {
                                        Debug.Log(JointArray[i].gameObject.name + " is attached to " + Atom1rb.gameObject.name);
                                        PV.RPC("DeleteJointOverNetworkByIndex", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID, i);
                                    }

                                    BondingPartnerList.Remove(Atom1rb);
                                }
                            }

                            for (int i = 1; i < 5; i++)  //iterate the search so that distant contacts will be included in the BondingPartnerList
                            {
                                foreach (GameObject atom in CD.GSC.GetElementsAtGivenPosition(MolIDValue))  //This is to move the hydrogens attached to carbons
                                {
                                    //THIS SCRIPT NOW LOOKS AT EVERY JOINT ON A POLYVALENT ATOM!!!!
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

                            // Step: remove Atom2 from inventory?

                            if (BondingPartnerList.Count == 1)
                            {
                                // Atom2 inventory removal handled by TransferSingleElement...no need to worry
                                PV.RPC("ChangeBondingState", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID, false);
                                CD.GameSetupPhotonView.RPC("TransferSingleElement", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID, 0);
                            }

                            else
                            {
                                int[] GameObjectBondingPartnerList = new int[BondingPartnerList.Count];

                                for (int i = 0; i < BondingPartnerList.Count; i++)
                                {
                                    GameObjectBondingPartnerList[i] = BondingPartnerList[i].gameObject.GetComponent<AtomController>().PVID;
                                }

                                CD.GameSetupPhotonView.RPC("RemoveSetOfElements", RpcTarget.All, GameObjectBondingPartnerList);
                                CD.GameSetupPhotonView.RPC("GenerateBatchID", RpcTarget.All, GameObjectBondingPartnerList);
                            }

                            if (CD.GSC.NumElementsInMolecule(MolIDValue) == 1)
                            {
                                CD.GameSetupPhotonView.RPC("TransferSingleElement", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID, 0);
                                PV.RPC("ChangeBondingState", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID, false);
                            }

                            // This isn't quite working...
                            PV.RPC("MoveAtomsAndAdjustValleys", RpcTarget.All, collider.tag, Atom1.GetComponent<AtomController>().PVID, Atom2.GetComponent<AtomController>().PVID);
                        }
                    }
                }
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        FrameActiveCount++;
        if (FrameActiveCount == WaitABit)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    private void DeleteJointOverNetwork(int PVID)
    {
        PhotonView.Find(PVID).GetComponent<FixedJoint2D>().connectedBody = null;
        Destroy(PhotonView.Find(PVID).GetComponent<FixedJoint2D>());
    }

    [PunRPC]
    private void DeleteJointOverNetworkByIndex(int PVID, int JointArrayIndex)
    {
        PhotonView.Find(PVID).GetComponents<FixedJoint2D>()[JointArrayIndex].connectedBody = null;
        Destroy(PhotonView.Find(PVID).GetComponents<FixedJoint2D>()[JointArrayIndex]);
        Debug.Log("Deleting joint index " + JointArrayIndex);
    }

    [PunRPC]
    private void ChangeBondingState(int PVID, bool state)
    {
        PhotonView.Find(PVID).GetComponent<AtomController>().isBonded = state;
    }

    [PunRPC]
    private void MoveAtomsAndAdjustValleys(string ColliderTag, int Atom1PVID, int Atom2PVID)
    {
        GameObject Atom1 = PhotonView.Find(Atom1PVID).gameObject;
        GameObject Atom2 = PhotonView.Find(Atom2PVID).gameObject;
        //the line of code below moves the unbonded atoms apart by a reasonable distance
        bondDirection = (Atom2.transform.position - Atom1.transform.position); //finds the vector that lines up the two atoms
        Atom2.transform.position = new Vector2(Atom2.transform.position.x + 0.23f * bondDirection.x, Atom2.transform.position.y + 0.23f * bondDirection.y);
        if (ColliderTag == "UnbondingTriggerDB")
        {
            Atom1.GetComponent<AtomController>().BondingOpportunities += 2;
            Atom2.GetComponent<AtomController>().BondingOpportunities += 2;
        }
        else
        {
            Atom1.GetComponent<AtomController>().BondingOpportunities++;
            Atom2.GetComponent<AtomController>().BondingOpportunities++;
        }
        DontBondAgain = 20;
        print("DontBondAgain set to 20");
    }

}
