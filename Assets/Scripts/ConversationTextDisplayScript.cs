using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ConversationTextDisplayScript : MonoBehaviour
{
    private Text ConversationTextBox;
    private bool unbondingMessageAlreadyGiven;
    // If ConversationTextBox.text and lastTextDisplaying differ, then synchronize the other player's display
    private string lastTextDisplaying;

    public static ConversationTextDisplayScript Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        ConversationTextBox = GetComponent<Text>();
        unbondingMessageAlreadyGiven = false;

        ChangeUnbondingText.Instance.OnMouseClickUnbond += FirstTimeUnbonding;

        // Singleton design pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

    // Called when no more of an element can be drawn (the user has the option to pick another)
    public void OutOfInventory2()
    {
        ConversationTextBox.text = "You can't have any more of that atom, choose another!";
        StartCoroutine(countdown());
    }

    public void OutOfInventory3()
    {
        ConversationTextBox.text = "You can't swap that atom, you already are at the Limit!";
        StartCoroutine(countdown());
    }

    public void finalTurn()
    {
        ConversationTextBox.text = "Final turn! The next time you press the die, the game will be over!";
        StartCoroutine(countdown());
    }

    public void noStack()
    {
        ConversationTextBox.text = "Don't stack atoms on top of each other!";
        StartCoroutine(countdown());
    }

    public void NoBondToBreak()
    {
        ConversationTextBox.text = "This Bond is Already Broken";
        StartCoroutine(countdown());
    }

    public void FirstTimeUnbonding()
    {
        if (!unbondingMessageAlreadyGiven)
        {
            ConversationTextBox.text = "Click a bond to unbond it!";
            StartCoroutine(countdown());
            unbondingMessageAlreadyGiven = true;
        }
    }

    public void ForcedClear()
    {
        ConversationTextBox.text = "";
    }

    private IEnumerator countdown()  //this is a co-routine, can run in parallel with other scripts/functions
    {
        yield return new WaitForSeconds(5);
        ConversationTextBox.text = "";
        yield break;
    }

    private void Update()
    {
        if (ConversationTextBox.text != lastTextDisplaying)
        {
            GetComponent<PhotonView>().RPC("SyncText", RpcTarget.Others, ConversationTextBox.text);
            lastTextDisplaying = ConversationTextBox.text;
        }
    }

    [PunRPC]
    private void SyncText(string value)
    {
        ConversationTextBox.text = value;
    }
}
