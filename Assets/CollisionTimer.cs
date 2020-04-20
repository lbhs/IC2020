using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTimer : MonoBehaviour
{
	Collider hitreg;
	public int timer;
    // Start is called before the first frame update
    void Start()
    {
        hitreg = GetComponent<Collider>();
		timer = 40;
		
    }

    // Update is called once per frame
    void Update()
    {
        timer -= 1;
		if(timer == 0)
		{
			hitreg.enabled = !hitreg.enabled;
		}
    }
}
