using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassMovement : MonoBehaviour
{
	private Vector3 velocity;
	public Rigidbody glass;
	public int toggle;
	public int timer;
    // Start is called before the first frame update
    void Start()
    {
        glass = gameObject.GetComponent<Rigidbody>();
		toggle = -1;
		
    }

    // Update is called once per frame
    void Update()
    {
		
		if(transform.position.x >= 16)
		{
			toggle = -1;
		}
		if(transform.position.x <= -10)
		{
			toggle = 1;
		
		}
        glass.MovePosition(transform.position + transform.right * Time.fixedDeltaTime * 25 * toggle);
		
		
  }
	
}
