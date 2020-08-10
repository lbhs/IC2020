using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldenJewelMover : MonoBehaviour
{

    
    private int i;
    private Vector3 targetForGoldenJewels;
    
    private float JouleCostPerAtom;
    private Vector2 bondDirection;
    private AudioSource SoundFX2;

    public GameObject FlameIcon;
    


    // Start is called before the first frame update
    void Start()   
    {
        SoundFX2 = GameObject.Find("BondBrokenSound").GetComponent<AudioSource>();
        

    }

    public void MovingJewel(int JouleCost, GameObject Atom1, GameObject Atom2)
    {
        //print("time to move Golden Jewels!");
        //print("Atom1 = " + Atom1);
        //print("Atom2 = " + Atom2);
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOff();  //only one flame at a time
        JouleCostPerAtom = JouleCost / 2f;
        targetForGoldenJewels = GameObject.FindWithTag("UnbondingJoule").transform.position;  //destination for moving GoldenJewel 
        //print(targetForGoldenJewels);
        StartCoroutine(MoveJewel(JouleCost, Atom1, Atom2));

    }

    public void DiatomicDissociation(GameObject Diatomic, int JouleCost)
    {
        print("Golden jewels moving to dissociate diatomic molecule");
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOff();
        targetForGoldenJewels = GameObject.FindWithTag("UnbondingJoule").transform.position;  //destination for moving GoldenJewel 
        //print("Target location = " + targetForGoldenJewels);
        StartCoroutine(MoveJewelsToDiatomic(Diatomic, JouleCost)); 
    }

    // Update is called once per frame
    void Update()
    {  

    }

    //MoveJewel IEnumerator is used for dissociation of a "normal" bond
    private IEnumerator MoveJewel(int JouleCost, GameObject Atom1, GameObject Atom2) 
    {        
        while (transform.position != targetForGoldenJewels)     //moves GoldenJewels to site of unbonding event        
        {
            JewelMover.JewelsInMotion = true;
            transform.position = Vector3.MoveTowards(transform.position, targetForGoldenJewels, 30*Time.deltaTime);   //third value is the speed at which Jewel moves
            yield return new WaitForEndOfFrame();  
        }

        print("The Golden Jewel has arrived");
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOn();

        JewelMover.JewelsInMotion = false;
        //THE UNBONDING OF ATOMS IS DELAYED UNTIL THE GOLDEN JEWELS ARRIVE!!
            print(Atom1 +" PE increased by" + JouleCostPerAtom);
            Atom1.GetComponent<PotentialEnergy>().PE += JouleCostPerAtom;    //adds potential energy to unbonded atoms!
            Atom2.GetComponent<PotentialEnergy>().PE += JouleCostPerAtom;
            Atom1.GetComponent<PotentialEnergy>().PotentialEnergyAdjust();
            Atom2.GetComponent<PotentialEnergy>().PotentialEnergyAdjust();

        //Atom GameObjects are physically moved apart in the following section
        bondDirection = (Atom2.transform.position - Atom1.transform.position); //finds the vector that lines up the two atoms
        Atom2.transform.position = new Vector2(Atom2.transform.position.x + 0.23f * bondDirection.x, Atom2.transform.position.y + 0.23f * bondDirection.y);
        Atom1.GetComponent<BondMaker>().valleysRemaining++;   //an empty bonding slot has appeared on Atom1
        Atom2.GetComponent<BondMaker>().valleysRemaining++;    //an empty bonding slot has appeared on Atom2
        SoundFX2.Play();   //NEED A TRANSFORMATION OF JOULES SOUND--KE HAS BECOME PE!!!  MAGIC!!!
        GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().HeatToPEConversion(JouleCost);
        //GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().AdjustJoulesInCorral();

        UnbondingScript2.DontBondAgain = 20;  //
        print("DontBondAgain set to 20");


        Destroy (gameObject);

        yield return null;


    }

    //MoveJewelsToDiatomic accomplishes dissociation of original diatomic molecules
    private IEnumerator MoveJewelsToDiatomic(GameObject Diatomic, int JouleCost)
    {        
        while (transform.position != targetForGoldenJewels) 
        {
            transform.position = Vector3.MoveTowards(transform.position, targetForGoldenJewels, 30*Time.deltaTime);   //third value is the speed at which Jewel moves
            yield return new WaitForEndOfFrame();
        }


        print("The Golden Jewel has arrived");
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOn();

        Instantiate(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(Diatomic.transform.position.x - 1.0f, Diatomic.transform.position.y, 0f), Quaternion.identity);  //Dissociation Produce is a public GameObject defined in DiatomicScript (H, Cl or O)
        Instantiate(Diatomic.GetComponent<DiatomicScript>().DissociationProduct, new Vector3(Diatomic.transform.position.x + 1.2f, Diatomic.transform.position.y, 0f), Quaternion.Euler(0f, 0f, 180f));
        Destroy(Diatomic);  //the line above spawns the second atom rotated 180  degrees from the first
        SoundFX2.Play();   //Bond breaking soundDestroy(Joule);
        //print("Case 1 (diatomic) complete");
        //GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().AdjustJoulesInCorral();

        Destroy(gameObject);  //the Unbonding Jewel is now destroyed

        if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 3)
        {
            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().BreakingBondsIncreasesPE();
        }
        else
        {
            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().HeatToPEConversion(JouleCost);
        }

        yield return null;


    }

}
 