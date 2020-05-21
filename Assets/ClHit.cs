using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClHit : MonoBehaviour
{
	
	public Scoreboard scoreboard;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		
    }
	void OnCollisionEnter (Collision collider)
	{
		if(collider.gameObject.tag == "Cl atom" || collider.gameObject.tag == "HCl")
		{
			
			scoreboard.MultiRes();
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            StartCoroutine(ExecuteAfterTime(0.05f));
			Destroy(collider.gameObject);
        }
	}
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
