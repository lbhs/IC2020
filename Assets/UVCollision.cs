using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UVCollision : MonoBehaviour
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
		if(collider.gameObject.tag == "UV")
		{
			Destroy(collider.gameObject);
			scoreboard.MultiDown();
		}
	}
}
