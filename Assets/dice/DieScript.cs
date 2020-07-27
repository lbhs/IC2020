using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour {

	Rigidbody rb;
	public Vector3 dieVelocity;
	private Vector3 startPos;
	private Quaternion startRot;
	public static int rolling = 0;
    public static int totalRolls = 0; //reset to 0 when kenneth is done working on gameover screen
    public AudioSource DieRoll;
    public static bool BondMessageGiven;
    public static int RotateMessageGiven;  //this variable is altered in RotateThis script
    public static int SwitchAtomMessageGiven;  //this variable is altered in SwapIt script


	void Start()
	{
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
		startRot = transform.rotation;
        totalRolls = 0; //reset to 0
        DieRoll = GameObject.Find("DieRollSound").GetComponent<AudioSource>();
    }
	
	void Update()
	{
		dieVelocity = rb.velocity; //looks useless but very important for other scripts
    }

    public void RollDiceAnimation()
    {        
        if (rolling >= 4)
        {
            rolling = 0;
        }
        if(rolling == 0)
        {
            if (totalRolls < 12)
            {
                Roll();
				rolling++;
				totalRolls++;
                StopAllCoroutines(); //only allows one instance of the countdown at a time, the most recent one
                StartCoroutine(countdown());
            }
            else  //this occurs when game is over!
            {
                //NEED TO CLEARLY MARK THE LAST TURN--PLAYER NEEDS TO GET AS MANY PTS AS POSSIBLE PRIOR TO "GAME OVER"
                GetComponent<resetScene>().gameOver();
            }
        }
    }

    public void Reset()
	{
		transform.position = startPos;
		transform.rotation = startRot;
	}

    private IEnumerator countdown()  //this is a co-routine, can run in parallel with other scripts/functions
    {
        yield return new WaitForSeconds(5);
        if (rolling == 1)
        {
            Roll();
			Debug.Log("try again");
            StartCoroutine(countdown());
        }
        yield break;
    }
	
	public void Roll()
	{
        if (JewelMover.JewelsInMotion == true)  //disable die roll while jewels are moving
        {
            print("can't roll while Jewels are Moving!");
            return;
        }  

        //print("roll function activated");
        GameObject.Find("UI").GetComponent<Animator>().SetBool("Exiting", false);
		DieRoll.Play();
		rb.velocity = Vector3.zero;
		float dirX = Random.Range(-500, 500);
		float dirY = Random.Range(-500, 500);
		float dirZ = Random.Range(-500, 500);
		transform.position = startPos;
		transform.rotation = Random.rotation;
		rb.AddForce(new Vector3(Random.Range(-100f, 100f), 1000, 0));
		rb.AddTorque(dirX, dirY, dirZ);
	}
}
