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
    public Texture TutorialDieWrapper;
    public Texture ElementLettersDieWrapper;
    public Renderer DieRenderer;


	void Start()
	{
        DieRenderer.material.SetTexture("_MainTex", ElementLettersDieWrapper);
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
				totalRolls++;
				int r = 0;
				if (totalRolls == 2 && GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true) //active if this is a tutorial round
				{
					r = 3;
				}
				else if (totalRolls == 3 && GameObject.Find("TutorialMarker").GetComponent<TutorialScript>().Tutorial == true)
				{
					r = 5;
				}
                Roll(r);
				rolling++;
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
	
	//function that physically rolls the die
	//optional parameter r corresponds to rigging a roll to a certain number, will do random if none provided
	public void Roll(int r = 0)
	{
        if (JewelMover.JewelsInMotion == true)  //disable die roll while jewels are moving
        {
            print("can't roll while Jewels are Moving!");
            return;
        }
		
		//initially give randomized values in case invalid roll passed to function
		Quaternion rotation = Random.rotation;
		Vector3 torque = new Vector3(Random.Range(-500, 500), Random.Range(-500, 500), Random.Range(-500, 500));
		float direction = Random.Range(-100f, 100f);
		
		//if we were given an r value, overwrite the random values with predetermined rolls to rig the die
		switch(r)
		{
			case 1:
				rotation = new Quaternion(0.8037788f, 0.4322418f, 0.3663995f, -0.1812677f);
				torque = new Vector3(379, -298, 272);
				direction = 68.16382f;
				break;
			case 2:
				rotation = new Quaternion(0.6919715f, 0.03455772f, 0.4860313f, 0.5326865f);
				torque = new Vector3(448, 420, -347);
				direction = 79.48134f;
				break;
			case 3:
				rotation = new Quaternion(0.8122342f, -0.1806391f, -0.4801453f, -0.2776790f);
				torque = new Vector3(-108, 10, -265);
				direction = 98.50467f;
				break;
			case 4:
				rotation = new Quaternion(0.1448607f, 0.7198941f, -0.3440828f, -0.5851281f);
				torque = new Vector3(-223, -461, -459);
				direction = -8.009544f;
				break;
			case 5:
				rotation = new Quaternion(0.01642922f, 0.6816381f, 0.1620304f, 0.7133343f);
				torque = new Vector3(406, 187, -88);
				direction = 82.8094f;
				break;
			case 6:
				rotation = new Quaternion(0.6754952f, 0.3663176f, -0.2509511f, -0.5886775f);
				torque = new Vector3(208, 366, -409);
				direction = 69.2898f;
				break;
		}
		
		/* logging all randomized values
		print(rotation.w);
		print(rotation.x);
		print(rotation.y);
		print(rotation.z);
		print(torque.x);
		print(torque.y);
		print(torque.z);
		print(direction);
		*/
		
        //print("roll function activated");
        GameObject.Find("UI").GetComponent<Animator>().SetBool("Exiting", false);
		DieRoll.Play();

		//assign randomized or rigged values of initial rotation, torque, and direction of toss
		rb.velocity = Vector3.zero;
		transform.position = startPos;
		transform.rotation = rotation;
		rb.AddForce(new Vector3(direction, 1000, 0));
		rb.AddTorque(torque);
	}
}
