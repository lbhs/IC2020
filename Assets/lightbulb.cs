using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightbulb : MonoBehaviour
{
	public Sprite off;
	public Sprite on;
	
    void Update()
    {
		GetComponent<SpriteRenderer>().sprite = off;
		transform.localScale = new Vector3(0.5f, 0.5f, 1f);
		transform.position = new Vector3(1.92f, 6.47f, 0f);
		
        foreach(GameObject particle in GameObject.Find("GameObject").GetComponent<forces>().rootObjects)
		{
			if(particle.name == "[P] electron(Clone)" && Vector2.Distance(transform.position, particle.transform.position) < 3f)
			{
				GetComponent<SpriteRenderer>().sprite = on;
				transform.localScale = new Vector3(0.15f, 0.15f, 1f);
				transform.position = new Vector3(1.92f, 6.67f, 0f);
			}
		}
    }
}
