using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannotBreakBond : MonoBehaviour
{
    public Text NotEnoughJoulesTextbox;
	public static bool final = false;

    // Start is called before the first frame update
    void Start()
    {
		final = false;
        NotEnoughJoulesTextbox.text = null;
    }

    public void Denied()
    {
        NotEnoughJoulesTextbox.text = "You don't have enough Joules to break this bond!";
        StartCoroutine(countdown());
    }
	
	public void OutOfInventory()
	{
		NotEnoughJoulesTextbox.text = "There aren't enough atoms left for that roll, roll again!";
        StartCoroutine(countdown());
	}
	
	public void OutOfInventory2()
	{
		NotEnoughJoulesTextbox.text = "You can't have any more of that atom, choose another!";
        StartCoroutine(countdown());
	}
	
	public void OutOfInventory3()
	{
		NotEnoughJoulesTextbox.text = "You can't swap that atom, you alrady have too many!";
        StartCoroutine(countdown());
	}
	
	public void finalTurn()
    {
		final = true;
        NotEnoughJoulesTextbox.text = "Final Turn! Roll again to end the game!";
        StartCoroutine(countdown());
    }
	
	public void noStack()
    {
        NotEnoughJoulesTextbox.text = "Don't try to stack atoms on top of existing ones!";
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



