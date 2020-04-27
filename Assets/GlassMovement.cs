using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassMovement : MonoBehaviour
{
	public Vector3 velocity;
	public Rigidbody glass;
	public int toggle;
	public int timer;
    // Start is called before the first frame update
    void Start()
    {
        glass = gameObject.GetComponent<Rigidbody>();
		toggle = 1;
		timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
		timer = timer - 1;
		if(timer == 0)
		{
			toggle = 1;
		}
		if(timer == -220)
		{
			toggle = -1;
			timer = 220;
		}
        glass.MovePosition(transform.position + transform.right * Time.fixedDeltaTime * 25 * toggle);
		
		
  }
	
}
