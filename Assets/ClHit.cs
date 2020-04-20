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
		}
	}
}
