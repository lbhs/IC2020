using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeScript : MonoBehaviour
{
    public Vector3 velocity;
    public int velocitySpeedUp;
    // Start is called before the first frame update
    void OnEnable()
    {
        gameObject.GetComponent<Rigidbody>().velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < velocitySpeedUp)
        {
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity*1.4f;
        }
    }
}
