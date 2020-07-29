using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationTextDisplayScript: MonoBehaviour
{
    public Text ConversationTextBox;
    public static bool final = false;
    public Color PEtoHeat;
    public Color HeattoPE;
    public Color DieColor;
    public Color ScoreColor;
    public GameObject DiceActiveOrNot;
	public Sprite endgametextsprite;

    // Start is called before the first frame update
    void Start()
    {
        final = false;
        if(GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true)
        {
            DragOutAtomTutorial();  //this starts the tutorial version!  The first 3 turns are scripted.
        }
        else
        {
            StartRolling();
        }

    }

    public void DragOutAtom()  //the function is called from DieCheckZoneScript (line 73)
    {
        ConversationTextBox.text = "Drag an atom into the play space";
        ConversationTextBox.color = Color.white;
        ConversationTextBox.fontStyle = FontStyle.Bold;
        StartCoroutine(countdown());
    }

    public void DragOutAtomTutorial()  //the function is called from above (conversationTextDisplayScript)
    {
        ConversationTextBox.text = "Drag a Carbon atom into the play space";
        ConversationTextBox.color = Color.white;
        ConversationTextBox.fontStyle = FontStyle.Bold;
        //StartCoroutine(countdown());
    }

    public void RotateAtomInstructions()  //used in the tutorial version--called from 
    {
        ConversationTextBox.text = "Tap on an atom to rotate it";
        ConversationTextBox.color = Color.blue;
        ConversationTextBox.fontStyle = FontStyle.Bold;
        //this text remains visible until the player has successfully rotated an atom (in RotateThis script)

    }

    public void SwitchAtomForm()
    {
        ConversationTextBox.text = "Use a long tap to change the form of a carbon or oxygen atom";
        ConversationTextBox.color = Color.yellow;
        //this text remains visible until player switches atom form (in SwapIt script)
    }

    public void ChooseOxygenEB()  //this is used only in tutorial mode--called from DieCheckZone script
    {
        ConversationTextBox.text = "Choose the SECOND form of oxygen (the diagonal form)";
        ConversationTextBox.color = PEtoHeat;
    }

    public void BlueJoulesArePE()  //called in tutorial mode from SwapIt script after player has changed atom form a couple times
    {
        ConversationTextBox.text = "Blue Jewels represent POTENTIAL ENERGY.";
        ConversationTextBox.color = Color.blue;
        //TRY TO MAKE THE JEWELS BLINK ON THE CARBON ATOM WHILE THIS TEXT IS DISPLAYED!!!!
        Invoke("CarbonHas4JoulesPE", 5);
    }

    public void CarbonHas4JoulesPE()  //called in tutorial mode from the function shown above (BlueJoulesArePE)
    {
        ConversationTextBox.text = "An UNBONDED carbon atom has 4-Joules of Potential Energy";
        ConversationTextBox.color = Color.white;
        Invoke("StartRolling", 4);
    }

    public void StartRolling()  //called in tutorial mode from the function shown above (CarbonHas4JoulesPE)
    {
        GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().DiceActiveOrNot.SetActive(true);  //this makes the purple die appear in tutorial
        ConversationTextBox.text = "Click the Purple Dice Icon to roll the Die";
        ConversationTextBox.fontStyle = FontStyle.Bold;
        ConversationTextBox.color = DieColor;
        DieScript.rolling = 0;
    }

    public void ContinueRolling()  //called in tutorial mode from the function shown above (CarbonHas4JoulesPE)
    {
        GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().DiceActiveOrNot.SetActive(true);
        ConversationTextBox.text = "Click the Purple Die Icon to Continue";
        ConversationTextBox.fontStyle = FontStyle.Bold;
        ConversationTextBox.color = DieColor;
        //DieScript.rolling = 0;
    }

    public void SwitchFormToOxygenEB()
    {
        ConversationTextBox.text = "Switch the oxygen's form to a diagonal form to allow it to double bond with Carbon";
        ConversationTextBox.color = PEtoHeat;
        ConversationTextBox.fontStyle = FontStyle.Bold;
        GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().DiceActiveOrNot.SetActive(false);
        Invoke("BondingConvertsPEtoHeat", 8);
    }

    public void BondingConvertsPEtoHeat()
    {
        GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().DiceActiveOrNot.SetActive(false);
        ConversationTextBox.text = "When atoms bond, Potential Energy is converted to Heat Energy.";
        ConversationTextBox.color = PEtoHeat;
        Invoke("MakeDoubleBond", 6);
    }

    public void MakeDoubleBond()
    {
        ConversationTextBox.text = "Rotate the Oxygen and Line up the atoms on a diagonal to make a Double Bond!";
        ConversationTextBox.color = Color.blue;
    }

    public void BondStrengthMessage()
    {
        ConversationTextBox.text = "The number of Joules converted to Heat depends on BOND STRENGTH (see the red onscreen table)";
        ConversationTextBox.color = PEtoHeat;
        Invoke("PointsForRedJoules", 7);
        
    }

    public void PointsForRedJoules()
    {
        ConversationTextBox.text = "Each Joule of Heat is Worth 10 Points. You now have: " +DisplayCanvasScript.ScoreTotal + " points!";
        ConversationTextBox.color = ScoreColor;
        Invoke("ContinueRolling", 6);
    }

    public void ChooseH2()
    {
        GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().DiceActiveOrNot.SetActive(false);
        ConversationTextBox.text = "Choose diatomic hydrogen (H2)";
        ConversationTextBox.color = Color.white;
    }

    public void BreakABond()
    {
        GameObject.Find("FlameController").GetComponent<FlameHidingScript>().FlameOn();
        ConversationTextBox.text = "Drag the Bond-breaking Energy Flame Icon onto the center of the H2 molecule";
        ConversationTextBox.color = ScoreColor;
    }

    public void BreakingBondsIncreasesPE()
    {
        print("got here");
        ConversationTextBox.text = "Breaking a Bond converts Heat Energy to Potential Energy";
        ConversationTextBox.color = Color.blue;
        Invoke("BondHtoC", 7);
    }

    public void BondHtoC()
    {
        ConversationTextBox.text = "Bond both of the single Hydrogen Atoms to the Carbon Atom";
        ConversationTextBox.color = Color.white;

    }

    public void MoleculeCompletionEarnsBonusPts()
    {
        ConversationTextBox.text = "Completing a Molecule earns Bonus Points (see blue table for point values)";
        ConversationTextBox.color = Color.blue;
        
        if(GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls==3)
        {
            Invoke("ContinueToTheEndOfTheGame", 7);
        }
        else
        {
            StartCoroutine(countdown());
        }
        
    }

    public void ContinueToTheEndOfTheGame()
    {
        print("Continue to the End");
        //StopAllCoroutines();
        ConversationTextBox.text = "A complete game is 12 rounds.";
        ConversationTextBox.color = Color.yellow;
        ConversationTextBox.fontStyle = FontStyle.Bold;
        Invoke("ContinueToTheEndOfTheGame2", 7);
    }

    public void ContinueToTheEndOfTheGame2()
    {
        ConversationTextBox.text = "To get a high score, make STRONG BONDS and COMPLETE many molecules!";
        Invoke("ContinueRollingWithCountdown", 7);
    }


    public void ContinueRollingWithCountdown()
    {
        GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().DiceActiveOrNot.SetActive(true);
        ConversationTextBox.text = "Click the Purple Die to Start the next turn";
        ConversationTextBox.fontStyle = FontStyle.Bold;
        ConversationTextBox.color = DieColor;
        DieScript.rolling = 0;
        StartCoroutine(countdown());
    }

    public void Denied()
    {
        ConversationTextBox.text = "You don't have enough Joules to break this bond!";
        StartCoroutine(countdown());
    }

    public void OutOfInventory()
    {
        ConversationTextBox.text = "These atoms are exhausted.  Roll again!";
        StartCoroutine(countdown());
    }

    public void OutOfInventory2()
    {
        ConversationTextBox.text = "You can't have any more of that atom, choose another!";
        StartCoroutine(countdown());
    }

    public void OutOfInventory3()
    {
        ConversationTextBox.text = "You can't swap that atom, you alrady are at the Limit!";
        StartCoroutine(countdown());
    }

    public void finalTurn()
    {
        final = true;
        ConversationTextBox.text = "Final Turn! Make all your moves, then click END GAME to finalize your score!";
		GameObject.Find("DiceButton").GetComponent<Image>().sprite = endgametextsprite;
        ConversationTextBox.color = Color.yellow;
		StartCoroutine(countdown());
    }

    public void noStack()
    {
        ConversationTextBox.text = "Drag your chosen atom into an EMPTY SPACE in the playing field.";
        ConversationTextBox.color = Color.yellow;
        StartCoroutine(countdown());
    }

    public void NoBondToBreak()
    {
        ConversationTextBox.text = "This Bond is Already Broken";
        StartCoroutine(countdown());
    }

    public void HeatToPEConversion(int JouleCost)
    {
       ConversationTextBox.text = JouleCost.ToString() + " Joules of Heat Energy converted to Potential Energy";
       ConversationTextBox.color = HeattoPE;
       StartCoroutine(countdown());
    }

    public void PEtoHeatConversion(int BondEnergy)
    {
        ConversationTextBox.text = BondEnergy.ToString() + " Joules of Potential Energy converted to Heat";
        ConversationTextBox.color = PEtoHeat;
        ConversationTextBox.fontStyle = FontStyle.BoldAndItalic;
        
        StartCoroutine(countdown());
    }

   

    public void CantSwap()
    {
        ConversationTextBox.text = "You can only swap UNBONDED atoms";
        StartCoroutine(countdown());
    }

    public void CantRotate()
    {
        ConversationTextBox.text = "You can only rotate UNBONDED atoms";
        StartCoroutine(countdown());
    }

    private IEnumerator countdown()  //this is a co-routine, can run in parallel with other scripts/functions
    {
        yield return new WaitForSeconds(5);
        if (final)
        {
            ConversationTextBox.text = "Final Turn! Make all your moves, then click END GAME to finalize your score!";
            ConversationTextBox.color = Color.yellow;
			GameObject.Find("DiceButton").GetComponent<Image>().sprite = endgametextsprite;
        }
        
        else if (GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true && DieScript.totalRolls == 3)
        {
            //don't erase the Bonus Point text message
        }

        else
        {
            ConversationTextBox.text = null;
            ConversationTextBox.color = Color.white;
            ConversationTextBox.fontStyle = FontStyle.Bold;
        }
        yield break;
    }
}



