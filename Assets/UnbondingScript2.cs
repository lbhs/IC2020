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
    public static int DontBondAgain;
    public static int WaitABit;
    private JouleDisplayController JDC;
    private int FrameActiveCount;
    private GameSetupContrller GSC;
    private int CheckIndex;

    // Start is called before the first frame update
    void Start()
    {
        DotCount = 0;                //resets DotCount
        Joule = gameObject;
        JointArray = new FixedJoint2D[5];
        TempAtomList = new List<GameObject>();
        BondingPartnerList = new List<Rigidbody2D>();

        FrameActiveCount = 0;

        JDC = GameObject.Find("UI").transform.GetChild(2).GetComponent<JouleDisplayController>();

        GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();

        if (GetComponent<PhotonView>().Owner == PhotonNetwork.PlayerList[0])
        {
            CheckIndex = 0;
        }
        else
        {
            CheckIndex = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.tag == "UnbondingTrigger" || collider.tag == "UnbondingTriggerDB" || collider.tag == "Diatomic") && GetComponent<PhotonView>().IsMine)

        {
            print("DotCount =" + DotCount);
            print("root object =" + collider.transform.root.gameObject);
            //DIATOMIC UNBONDING IS THE FIRST CASE
            if (collider.tag == "Diatomic")  //This section breaks a diatomic element into two atoms
            {
                print("dissociate diatomic!");
                Diatomic = collider.transform.root.gameObject;
                if (Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy > JDC.TotalJoulesDisplaying[CheckIndex])
                {
                    Debug.Log("You don't have enough joules to break this bond!");
                    return;
                }
                else
                {
                    GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, -Diatomic.GetComponent<DiatomicScript>().BondDissociationEnergy);
                    GSC.GetComponent<PhotonView>().RPC("ChangeScoreUniformly", RpcTarget.All, 0, -10);   //diatomic molecule = 10 bonus pts, need to subtract when molecule is destroyed

                    DiatomicPosition = Diatomic.transform.position;
                    GSC.NetowrkSpawn(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(DiatomicPosition.x - 1.0f, DiatomicPosition.y));
                    GSC.NetowrkSpawn(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(DiatomicPosition.x + 1.2f, DiatomicPosition.y));
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
                    if (JouleCost > JDC.TotalJoulesDisplaying[CheckIndex])
                    {
                        Debug.Log("You don't have enough joules to break this bond!");
                        return;
                    }

                    GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, -JouleCost);

                    MolIDValue = Atom1.GetComponent<AtomController>().MoleculeID;

                    GSC.GetComponent<PhotonView>().RPC("ChangeScoreUniformly", RpcTarget.All, -JouleCost, -GSC.ReturnCompletionScore(MolIDValue));

                    if (GSC.NumElementsInMolecule(MolIDValue) == 2)
                    {
                        GSC.GetComponent<PhotonView>().RPC("ClearMoleculeList", RpcTarget.All, MolIDValue);
                        if (Atom1.GetComponent<FixedJoint2D>())    //remove the bond!
                        {
                            GetComponent<PhotonView>().RPC("DeleteJointOverNetwork", RpcTarget.All, Atom1.GetComponent<AtomController>().PVID);
                        }
                        if (Atom2.GetComponent<FixedJoint2D>())
                        {
                            GetComponent<PhotonView>().RPC("DeleteJointOverNetwork", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID);
                        }
                        return;
                    }

                    else if (Atom2.GetComponent<AtomController>().Monovalent)
                    {
                        GetComponent<PhotonView>().RPC("DeleteJointOverNetwork", RpcTarget.All, Atom2.GetComponent<AtomController>().PVID);
                        Atom2.GetComponent<AtomController>().MoleculeID = 0;
                        Atom2.GetComponent<AtomController>().isBonded = false;
                        GSC.GetComponent<PhotonView>().RPC("RemoveGivenElements", RpcTarget.All, new int[1] { Atom2.GetComponent<AtomController>().PVID });
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
            Destroy(gameObject);
        }
    }

    [PunRPC]
    private void DeleteJointOverNetwork(int PVID)
    {
        Destroy(PhotonView.Find(PVID).GetComponent<FixedJoint2D>());
    }

    [PunRPC]
    private void IncrementJDC(int AmountToIncrement)
    {
        JDC.TotalJoulesDisplaying[CheckIndex] += AmountToIncrement;
    }
}