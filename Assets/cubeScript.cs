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
        float vx = UnityEngine.Random.Range(-5, 6);
        float vy = Mathf.Sqrt(50 - (vx * vx));
        float vPlaceholder = UnityEngine.Random.Range(0, 2) *2 -1;
        //print(vPlaceholder);
        
      
        velocity = new Vector3(vx, vy *vPlaceholder, 0);

        gameObject.GetComponent<Rigidbody>().velocity = velocity;

        GameObject.Find("GameObject").GetComponent<forces>().cubeList.Add(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < 50)
        {
            //print("old velocity =" + velocity);
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.normalized * 5 * Mathf.Sqrt(2);
            //print("new velocity =" + velocity);
        }
    }
}
