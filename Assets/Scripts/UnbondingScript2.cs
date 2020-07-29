using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnbondingScript2 : MonoBehaviour
{
    #region Public Member Variables
    public FixedJoint2D[] JointArray = new FixedJoint2D[5];   //this stores the joints found on a given carbon atom (could be up to 4 joints)
    public static int DontBondAgain;
    public static int WaitABit;
    #endregion

    #region Private Member Variables
    private GameObject Atom1;      //Atom1 and Atom2 are the two atoms to unbond
    private GameObject Atom2;
    private int DotCount;       //This is used to make sure two dots are collided with--if only 1 dot, no bond to break!
    private Vector2 bondDirection;   //atoms unbond by moving Atom2 along the axis of the original bond
    private int MolIDValue;       //This variable gets the proper atom list from MoleculeList[].  Also used to push the atom list back in
    private GameObject SwapAtom;   //it is convenient to swap hydrogen to be Atom 2--this temporary variable allows for swapping Atom1 and Atom2
    private int JouleCost;     //the bond strength for the bond that is broken
    FixedJoint2D jointToBreak;   //this variable allows examination of each bond (joint) in the molecule (one at a time)
    List<Rigidbody2D> BondingPartnerList;  //This is a Rigidbody list--when tracing bonds, need to use Rigidbody (joints connect RB's)
    private GameObject Diatomic;  //Represents the diatomic being unbonded (case #1)
    private Vector3 DiatomicPosition;  //Used to calculate the position of disassociation products
    private AudioSource SoundFX2; 
    private int FrameActiveCount;
    private int CheckIndex;  //Which index of JouleDisplayController.TotalJoulesDisplaying should I be using to see the number of accumulated Joules?
    private PhotonView PV;
    private static readonly float MoveGameObjectsAwaySpeed = 5;  //The speed at which groups of atoms separate (used in the MoveAtom2 coroutine)
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        DotCount = 0;                //resets DotCount
        JointArray = new FixedJoint2D[5];
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
                if (Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy > JouleDisplayController.Instance.TotalJoulesDisplaying[CheckIndex])
                {
                    ConversationTextDisplayScript.Instance.Denied();
                    return;
                }
                else
                {
                    GameSoundEffectsController.Instance.Unbonding();
                    JouleDisplayController.Instance.GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, -Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy, Diatomic.GetComponent<PhotonView>().ViewID, -10);
                    DiatomicPosition = Diatomic.transform.position;
                    GameSetupContrller.Instance.NetowrkSpawn(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(DiatomicPosition.x - 1.0f, DiatomicPosition.y));
                    GameSetupContrller.Instance.NetowrkSpawn(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(DiatomicPosition.x + 1.2f, DiatomicPosition.y));
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

                int MolID1 = Atom1.GetComponent<AtomController>().MoleculeID;
                int MolID2 = Atom2.GetComponent<AtomController>().MoleculeID;

                if (MolID1 != MolID2)
                {
                    ConversationTextDisplayScript.Instance.NoBondToBreak();
                    return;
                }

                if (MolID1 == 0 || MolID2 == 0)  //means one of these atoms is not bonded to anything
                {
                    ConversationTextDisplayScript.Instance.NoBondToBreak();
                    return;
                }

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
                    if (collider.tag == "UnbondingTriggerDB")  //if double bond, need to use BondArrayID 4 (for double bonded carbon) or 5 (for double bonded oxygen)
                    {
                        JouleCost = BondEnergyValues.Instance.ComputeBondEnergy(Atom1.GetComponent<AtomController>().EnergyMatrixPosition + 3, Atom2.GetComponent<AtomController>().EnergyMatrixPosition + 3);
                    }
                    else
                    {
                        JouleCost = BondEnergyValues.Instance.ComputeBondEnergy(Atom1.GetComponent<AtomController>().EnergyMatrixPosition, Atom2.GetComponent<AtomController>().EnergyMatrixPosition);
                    }

                    print("JouleCost =" + JouleCost);  //JouleCost comes from the bondArray[], which is a copy of the Master Array attached to BondEnergyMatrix GameObject
                    if (JouleCost > JouleDisplayController.Instance.TotalJoulesDisplaying[CheckIndex])
                    {
                        ConversationTextDisplayScript.Instance.Denied();
                        return;
                    }

                    MolIDValue = Atom1.GetComponent<AtomController>().MoleculeID;

                    GetComponent<PhotonView>().RPC("DeleteBadge", RpcTarget.All, MolIDValue);

                    // It doesn't matter whether we pass the PVID of Atom 1 or 2 -- they both have the same owner 
                    JouleDisplayController.Instance.GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, -JouleCost, Atom1.GetComponent<AtomController>().PVID, -MoleculeIDHandler.Instance.ReturnCompletionScore(MolIDValue));

                    if (collider.tag == "UnbondingTriggerDB")
                    {
                        Atom1.GetComponent<AtomController>().BondingOpportunities += 2;
                        Atom2.GetComponent<AtomController>().BondingOpportunities += 2;
                    }
                    else
                    {
                        Atom1.GetComponent<AtomController>().BondingOpportunities++;
                        Atom2.GetComponent<AtomController>().BondingOpportunities++;
                    }

                    if (MoleculeIDHandler.Instance.NumElementsInMolecule(MolIDValue) == 2)
                    {
                        GameSoundEffectsController.Instance.Unbonding();
                        Debug.Log("Unbond two elements!");
                        MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("RemoveElementListByID", RpcTarget.All, MolIDValue);
                        PV.RPC("DeleteJointsBetweenAtom1Atom2", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID, Atom2.GetComponent<AtomController>().PVID);
                        Atom1.GetComponent<AtomController>().isBonded = false;
                        Atom2.GetComponent<AtomController>().isBonded = false;
                        MoveAtomsAndAdjustValleys();
                        return;
                    }

                    else if (Atom2.GetComponent<AtomController>().Monovalent)
                    {
                        GameSoundEffectsController.Instance.Unbonding();
                        Debug.Log("Atom2 is a monovalent!");
                        PV.RPC("DeleteJointsBetweenAtom1Atom2", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID, Atom2.GetComponent<AtomController>().PVID);
                        Atom2.GetComponent<AtomController>().isBonded = false;
                        MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("RemoveArrayOfElements", RpcTarget.All, new int[1] { Atom2.GetComponent<AtomController>().PVID });
                        MoveAtomsAndAdjustValleys();
                        return;
                    }

                    else
                    {
                        if (MoleculeIDHandler.Instance.NumElementsInMolecule(MolIDValue) > 2)
                        {
                            GameSoundEffectsController.Instance.Unbonding();
                            Debug.Log("Complex unbonding!");
                            Rigidbody2D Atom1rb = Atom1.GetComponent<Rigidbody2D>();
                            Rigidbody2D Atom2rb = Atom2.GetComponent<Rigidbody2D>();

                            if (Atom1.GetComponent<FixedJoint2D>() != null)
                            {
                                JointArray = Atom1.GetComponents<FixedJoint2D>();
                            }

                            BondingPartnerList.Add(Atom2.GetComponent<Rigidbody2D>());

                            if (Atom2.GetComponent<FixedJoint2D>() != null)
                            {
                                JointArray = Atom2.GetComponents<FixedJoint2D>();

                                for (int i = 0; i < JointArray.Length; i++)
                                {
                                    BondingPartnerList.Add(JointArray[i].connectedBody);
                                }

                                BondingPartnerList.Remove(Atom1rb);
                            }

                            PV.RPC("DeleteJointsBetweenAtom1Atom2", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID, Atom2.GetComponent<AtomController>().PVID);

                            for (int i = 1; i < 5; i++)  //iterate the search so that distant contacts will be included in the BondingPartnerList
                            {
                                foreach (GameObject atom in MoleculeIDHandler.Instance.GetElementsAtGivenPosition(MolIDValue))  //This is to move the hydrogens attached to carbons
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
                                Atom2.GetComponent<AtomController>().isBonded = false;
                                MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("TransferSingleElement", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID, 0);
                            }

                            else
                            {
                                int[] GameObjectBondingPartnerList = new int[BondingPartnerList.Count];

                                for (int i = 0; i < BondingPartnerList.Count; i++)
                                {
                                    GameObjectBondingPartnerList[i] = BondingPartnerList[i].gameObject.GetComponent<AtomController>().PVID;
                                }

                                MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("RemoveArrayOfElements", RpcTarget.All, GameObjectBondingPartnerList);
                                MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("GenerateBatchID", RpcTarget.All, GameObjectBondingPartnerList);
                            }

                            if (MoleculeIDHandler.Instance.NumElementsInMolecule(MolIDValue) == 1)
                            {
                                MoleculeIDHandler.Instance.GetComponent<PhotonView>().RPC("TransferSingleElement", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID, 0);
                                Atom1.GetComponent<AtomController>().isBonded = false;
                            }

                            MoveAtomsAndAdjustValleys();
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
            if (GetComponent<PhotonView>().IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    private void DeleteBadge(int MoleculeID)
    {
        foreach (GameObject GO in MoleculeIDHandler.Instance.GetElementsAtGivenPosition(MoleculeID))
        {
            foreach (Transform child in GO.transform)
            {
                if (child.gameObject.tag == "Badge")
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
    }

    [PunRPC]
    private void DeleteJointsBetweenAtom1Atom2(int Atom1PVID, int Atom2PVID)
    {
        FixedJoint2D[] joints = PhotonView.Find(Atom1PVID).GetComponents<FixedJoint2D>();
        foreach (FixedJoint2D joint in joints)
        {
            if (joint.connectedBody == PhotonView.Find(Atom2PVID).GetComponent<Rigidbody2D>())
            {
                Destroy(joint);
            }
        }

        FixedJoint2D[] jointsToAtom1 = PhotonView.Find(Atom2PVID).GetComponents<FixedJoint2D>();
        foreach (FixedJoint2D joint in jointsToAtom1)
        {
            if (joint.connectedBody == PhotonView.Find(Atom1PVID).GetComponent<Rigidbody2D>())
            {
                Destroy(joint);
            }
        }
    }

    private void MoveAtomsAndAdjustValleys()
    {
        bondDirection = (Atom2.transform.position - Atom1.transform.position); //finds the vector that lines up the two atoms
        Vector3 DesiredPosition = new Vector3(Atom2.transform.position.x + 0.23f * bondDirection.x, Atom2.transform.position.y + 0.23f * bondDirection.y);
        StartCoroutine(MoveAtom2(DesiredPosition));

        DontBondAgain = 20;
        print("DontBondAgain set to 20");
    }

    private IEnumerator MoveAtom2(Vector3 targetPosition)
    {
        while (Atom2.transform.position != targetPosition)
        {
            Atom2.transform.position = Vector3.MoveTowards(Atom2.transform.position, targetPosition, 0.25f);
            yield return new WaitForEndOfFrame();
        }
    }
}
