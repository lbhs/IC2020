using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieCheckZoneScript : MonoBehaviour {
	
	[SerializeField]
	public GameObject die;
	public GameObject GameSetup;

	Vector3 dieVelocity;
	
	public int dieNumber;

    [SerializeField]
    private AllAtomPrefabs AtomPrefabs;

    public List<GameObject>[] atoms;

    private void Start()
    {
        atoms = new List<GameObject>[]
        {
            new List<GameObject> {AtomPrefabs.Atoms[0]},
            new List<GameObject> {AtomPrefabs.Atoms[7], AtomPrefabs.Atoms[8]},
            new List<GameObject> {AtomPrefabs.Atoms[2], AtomPrefabs.Atoms[3]},
            new List<GameObject> {AtomPrefabs.Atoms[5]},
            new List<GameObject> {AtomPrefabs.Atoms[1], AtomPrefabs.Atoms[4], AtomPrefabs.Atoms[6]},
            new List<GameObject> {AtomPrefabs.Atoms[0], AtomPrefabs.Atoms[1], AtomPrefabs.Atoms[2],
                                  AtomPrefabs.Atoms[3], AtomPrefabs.Atoms[4], AtomPrefabs.Atoms[5],
                                  AtomPrefabs.Atoms[6], AtomPrefabs.Atoms[7], AtomPrefabs.Atoms[8]}
        };
    }

    // Update is called once per frame
    void FixedUpdate () {
		dieVelocity = die.GetComponent<DieScript>().dieVelocity;
		if(die.GetComponent<DieScript>().rolling == 1 && dieNumber != 0)
		{
			//send the outcome of the roll to the main program
			Debug.Log("roll: " + dieNumber.ToString());
            RollDiceDieCheckZoneScript();
			dieNumber = 0;
			die.GetComponent<DieScript>().rolling = 2;
			die.GetComponent<DieFade>().startFade();
		}
	}

	void OnTriggerStay(Collider col)
	{
		if(((dieVelocity == Vector3.zero) && die.transform.position.y > 0 && (die.GetComponent<DieScript>().rolling == 1) && die.transform.parent.name == "die_player1") || ((dieVelocity == Vector3.zero) && die.transform.position.y > -16 && (die.GetComponent<DieScript>().rolling == 1) && die.transform.parent.name == "die_player2"))
		{
			switch(col.gameObject.name)
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

    public void RollDiceDieCheckZoneScript()
    {
        bool able = false;

        foreach (GameObject atom in atoms[dieNumber - 1])
        {
            // If each atom in the current roll options cannot be drawn, then don't increment the turn
            if (AtomInventory.Instance.PrefabCanBeDrawn(atom.name, false))
            {
                able = true;
            }
        }

        if (able)
        {
            GameSetup.GetComponent<GameSetupContrller>().RollDice(dieNumber);
            // Only register a turn if an atom or molecule can be drawn
            TurnController.Instance.IncrementCountWrapper();
            // The end turn button should not be interactable because a valid turn has registered
            // This also avoids the issue where the player rolls the die and ends their turn before drawing an element
            GameObject.Find("UI").transform.GetChild(0).GetComponent<Button>().interactable = true;
        }
        else
        {
            ConversationTextDisplayScript.Instance.OutOfInventory();
            // Make the dice button interactable so the player can roll again
            GameObject.Find("UI").transform.GetChild(1).GetComponent<Button>().interactable = true;
        }
    }
}
