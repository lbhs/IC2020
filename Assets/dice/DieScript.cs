using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieScript : MonoBehaviour {

	Rigidbody rb;
	public Vector3 dieVelocity;
	private Vector3 startPos;
	private Quaternion startRot;
	public static int rolling = 0;
	public static int totalrolls = 0;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
		startRot = transform.rotation;
		rolling = 0;
		totalrolls = 0;
	}
	
	void Update()
	{
		dieVelocity = rb.velocity;
    }
	
    public void RollDiceAnimation()
    {
		if(rolling == 0)
		{
			if(totalrolls < 13)
			{
				rolling++;
				totalrolls++;
				rb.velocity = Vector3.zero;
				float dirX = Random.Range(-500, 500);
				float dirY = Random.Range(-500, 500);
				float dirZ = Random.Range(-500, 500);
				transform.position = startPos;
				transform.rotation = Random.rotation;
				rb.AddForce(new Vector3(Random.Range(-100f, 100f), 1000, 0));
				rb.AddTorque(dirX, dirY, dirZ);
				StartCoroutine(countdown());
			}
			else
			{
				GetComponent<resetScene>().gameOver();
			}
		}
    }
	
	private IEnumerator countdown()
	{
        yield return new WaitForSeconds(5);
		if(rolling == 1)
		{
			rb.velocity = Vector3.zero;
			float dirX = Random.Range(-500, 500);
			float dirY = Random.Range(-500, 500);
			float dirZ = Random.Range(-500, 500);
			transform.localPosition = startPos;
			transform.rotation = Random.rotation;
			rb.AddForce(new Vector3(Random.Range(-100f, 100f), 1000, 0));
			rb.AddTorque(dirX, dirY, dirZ);
			StartCoroutine(countdown());
		}
		yield break;
	}

	public void Reset()
	{
		transform.position = startPos;
		transform.rotation = startRot;
	}
}
