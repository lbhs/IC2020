using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveArrow : MonoBehaviour
{
	public Vector3 velocity;
	public Rigidbody cannon;

    // Start is called before the first frame update
    void Start()
    {
			cannon = gameObject.GetComponent<Rigidbody>();
			cannon.isKinematic = true;
			//xpos == 1;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right"))
		{
			if(transform.position.x <= 16)
			{
			cannon.MovePosition(transform.position + transform.right * Time.fixedDeltaTime * 35);
			//transform.position = new Vector3(-3.0f, 0.0f, 0.0f);
			}
    }
		if (Input.GetKey("left"))
		{
			if(transform.position.x >= -10)
			{
			cannon.MovePosition(transform.position - transform.right * Time.fixedDeltaTime * 35);
			}
		}
	}
}
