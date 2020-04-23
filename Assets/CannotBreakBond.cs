using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannotBreakBond : MonoBehaviour
{
    public Text NotEnoughJoulesTextbox;
	private static bool final = false;

    // Start is called before the first frame update
    void Start()
    {
		final = false;
        NotEnoughJoulesTextbox.text = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Denied()
    {
        NotEnoughJoulesTextbox.text = "You don't have enough Joules to break this bond!";
        StartCoroutine(countdown());
    }
	
	public void finalTurn()
    {
		final = true;
        NotEnoughJoulesTextbox.text = "Final Turn! Roll again to end the game!";
        StartCoroutine(countdown());
    }

    private IEnumerator countdown()  //this is a co-routine, can run in parallel with other scripts/functions
    {
        yield return new WaitForSeconds(5);
		if(final)
		{
			NotEnoughJoulesTextbox.text = "Final Turn! Roll again to end the game!";
		} else {
			NotEnoughJoulesTextbox.text = null;
		}
        yield break;
    }
}



