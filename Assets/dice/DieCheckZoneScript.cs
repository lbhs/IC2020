using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieCheckZoneScript : MonoBehaviour {

	[SerializeField]
	public Animator UIAnim;
	public GameObject die;
	public static int dieNumber;
	Vector3 dieVelocity;
    public bool BondMessageGiven;
	public Sprite originaldiebuttonsprite;

	public GameObject hydrogen;
	public GameObject hydrogendiatomic;
	public GameObject oxygen;
	public GameObject oxygendouble;
	public GameObject oxygendiatomic;
	public GameObject chlorine;
	public GameObject chlorinediatomic;
	public GameObject carbon;
	public GameObject carbondouble;

	public List<GameObject>[] atoms;

	void Start()
	{
		atoms = new List<GameObject>[]
		{
			new List<GameObject> {hydrogen},
			new List<GameObject> {carbon, carbondouble},
			new List<GameObject> {oxygen, oxygendouble},
			new List<GameObject> {chlorine},
			new List<GameObject> {hydrogendiatomic, oxygendiatomic, chlorinediatomic},
			new List<GameObject> {hydrogen, hydrogendiatomic, oxygen, oxygendouble, oxygendiatomic, chlorine, chlorinediatomic, carbon, carbondouble}
		};

        if(GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true)
        {
            DieScript.totalRolls = 1;
            RollDice(2);   //good to start the game with a carbon atom!  Helps make an onscreen tutorial possible (rotate = tap, switch form = long tap)
        }
        
	}

	void FixedUpdate ()
	{
        //print("rolling value =" + DieScript.rolling);
		dieVelocity = die.GetComponent<DieScript>().dieVelocity;

		if(DieScript.rolling == 1 && dieNumber != 0)
		{
			//send the outcome of the roll to the main program
			Debug.Log("roll: " + dieNumber.ToString());

            //FOR TUTORIAL, 2ND ROUND = OXYGEN, 3RD ROUND = DIATOMIC
            if (DieScript.totalRolls == 2 && GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true) //active if this is a tutorial round
            {
                dieNumber = 3;
                print("Tutorial second round = oxygen");
                GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().ChooseOxygenEB();
            }

            else if (DieScript.totalRolls == 3 && GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true)
            {
                dieNumber = 5;
                print("Tutorial third round = diatomic");
                GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().ChooseH2();
            }

            else if (DieScript.totalRolls == 1)  //this instructs players what to do if they skip the tutorial
            {
                GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().DragOutAtom();
            }
            RollDice(dieNumber);   //set this back to dieNumber!!!  If set to 6, you always get choice of ALL atoms
			dieNumber = 0;
			DieScript.rolling++;                      
            die.GetComponent<DieFade>().startFade();
		}
	}

	void OnTriggerStay(Collider col)
	{
		if ((dieVelocity == Vector3.zero) && die.transform.position.y > -3 && (DieScript.rolling == 1))
		{
			switch (col.gameObject.name)
			{
				case "Side1":
					dieNumber = 6;
					break;
				case "Side2":
					dieNumber = 5;
					break;
				case "Side3":
					dieNumber = 4;
					break;
				case "Side4":
					dieNumber = 3;
					break;
				case "Side5":
					dieNumber = 2;
					break;
				case "Side6":
					dieNumber = 1;
					break;
			}
		}
	}

	public void RollDice(int Roll)
    {
		bool able = false;

		foreach(GameObject atom in atoms[Roll - 1])
		{
			if(AtomInventoryRemaining.removePiece(atom, false))
			{
				able = true;
			}
		}

		if(able)
		{
			if(Roll == 1)
			{
				UIAnim.SetTrigger("H");
                //PV.RPC("AnimateRollMenu", RpcTarget.All, "H");
			}
			else if (Roll == 2)
			{
				UIAnim.SetTrigger("C");
				//PV.RPC("AnimateRollMenu", RpcTarget.All, "O");
			}
			else if (Roll == 3)
			{
				UIAnim.SetTrigger("O");
				//PV.RPC("AnimateRollMenu", RpcTarget.All, "C");
			}
			else if (Roll == 4)
			{
				UIAnim.SetTrigger("CL");
				// PV.RPC("AnimateRollMenu", RpcTarget.All, "CL");
			}
			else if (Roll == 5)
			{
				UIAnim.SetTrigger("DoubleOnly");
				// PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleOnly");
			}
			else if (Roll == 6)
			{
				UIAnim.SetTrigger("DoubleDown");
				// PV.RPC("AnimateRollMenu", RpcTarget.All, "DoubleDown");
			}

                                      
        }

        else {
			GameObject.Find("ConversationDisplay").GetComponent<ConversationTextDisplayScript>().OutOfInventory();
			if(DieScript.totalRolls == 12)
			{
				ConversationTextDisplayScript.final = false;
				GameObject.Find("DiceButton").GetComponent<Image>().sprite = originaldiebuttonsprite;
			}
			DieScript.totalRolls--;
			DieScript.rolling++;
		}
    }
}
