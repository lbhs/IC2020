using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour {

	Rigidbody rb;
	public Vector3 dieVelocity;
	private Vector3 startPos;
	private Quaternion startRot;
	public int rolling = 0;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		startPos = transform.localPosition;
		startRot = transform.rotation;
		transform.Translate(Vector3.left * 40);
	}
	
	void Update()
	{
		dieVelocity = rb.velocity;
    }
	
    public void RollDiceAnimation()
    {
		if (rolling == 0)
		{
			rolling++;
			GetComponent<DieFade>().resetMaterial();
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
    }
	
	private IEnumerator countdown()
	{
        yield return new WaitForSeconds(5);
		if(GetComponent<DieScript>().rolling == 1)
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
		transform.Translate(Vector3.left * 40);
	}
}
