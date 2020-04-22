using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannotBreakBond : MonoBehaviour
{
    public Text NotEnoughJoulesTextbox;

    // Start is called before the first frame update
    void Start()
    {
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

    private IEnumerator countdown()  //this is a co-routine, can run in parallel with other scripts/functions
    {
        yield return new WaitForSeconds(5);
        NotEnoughJoulesTextbox.text = null;
        yield break;
    }
}



