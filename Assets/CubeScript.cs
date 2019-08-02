using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cubeScript : MonoBehaviour
{

    public Vector3 velocity;
    public float velocitySpeedUp;
    [HideInInspector] public float teamptureMutiplyer;
    private Slider tempatureSlider;
    // Start is called before the first frame update
    void OnEnable()
    {
        tempatureSlider = GameObject.Find("Teampture Slider").GetComponent<Slider>();
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
        teamptureMutiplyer = tempatureSlider.value;
        if (gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < velocitySpeedUp * teamptureMutiplyer)
        {
            //print("old velocity =" + velocity);
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.normalized * 5 * teamptureMutiplyer * Mathf.Sqrt(2);
            //print("new velocity =" + velocity);
        }
    }
}
