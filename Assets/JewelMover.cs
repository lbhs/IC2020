using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelMover : MonoBehaviour
{
    private int i;
    private Vector3 targetForMovingJewels;
    public static bool JewelsInMotion;  //disable rotation, atom shape swapping, unbonding, die roll while Jewels are in Motion

    // Start is called before the first frame update
    void Start()   
    {
        
    }

    public void MovingJewel(int BondEnergy)  //BondEnergy value is passed from BondMaker Script
    {
        print("BondEnergy passed from BondMaker = " + BondEnergy);
        targetForMovingJewels = GameObject.Find("JouleHolder").transform.position;  //destination for moving PEJewel formerly new Vector3(14, -6, 0)
        targetForMovingJewels = new Vector3(targetForMovingJewels.x, targetForMovingJewels.y, 0);
        print(targetForMovingJewels);
        StartCoroutine(MoveJewel(BondEnergy));
    }

    // Update is called once per frame
    void Update()
    {
        print("Jewels are in motion =" + JewelsInMotion);
    }

    private IEnumerator MoveJewel(int BondEnergy)
    {
        if(GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 2)
        {
            //tutorial explains that joules are determined by table of bond strengths
            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().BondStrengthMessage();
        }
        else
        {
            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().PEtoHeatConversion(BondEnergy);  //displays the energy change in a text box
        }
        

        while (transform.position != targetForMovingJewels) //moves the blue PE Jewels towards the Jewel Corral
            
        {
            JewelsInMotion = true;
            transform.position = Vector3.MoveTowards(transform.position, targetForMovingJewels, 30*Time.deltaTime);   //third value is the speed at which Jewel moves
            UnbondingScript2.DontBondAgain = 10;
            //print("DBA value" + UnbondingScript2.DontBondAgain);
            yield return new WaitForEndOfFrame();  
        }
        JewelsInMotion = false;
        
        
        for (i = 0; i < BondEnergy; i++)  //adds the right number of joules to the JouleCorral
        {
            //print("spawned red Jewels");
            GameObject.Find("JouleHolder").GetComponent<JouleHolderScript>().JSpawn();
        }
        
        if(BondMaker.MoleculeJustCompleted == true && DieScript.totalRolls < 12)
        {
            GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().MoleculeCompletionEarnsBonusPts();
            BondMaker.MoleculeJustCompleted = false;
        }

        Destroy (gameObject);  //this gameObject is the Moving Jewels Icon

        yield return null;

                        
    }



}
