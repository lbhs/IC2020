using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cubeScript : MonoBehaviour
{

    public Vector3 velocity;
    public float temp;
    private Rigidbody cube;
    private Slider temperatureSlider;

    // Start is called before the first frame update
    void OnEnable()
    {
        cube = gameObject.GetComponent<Rigidbody>();
        temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();

        float vx = UnityEngine.Random.Range(-5, 6);
        float vy = Mathf.Sqrt(50 - (vx * vx));
        velocity = new Vector3(vx, vy, 0);
        cube.velocity = velocity;
        temp = temperatureSlider.value;

        GameObject.Find("GameObject").GetComponent<forces>().nonObjects.Add(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        temp = temperatureSlider.value;

        if (Time.timeScale != 0 && GameObject.Find("GameObject").GetComponent<forces>().recording && cube.velocity.magnitude < (7 * temp))
        {
            cube.velocity *= 1.4f;
            print("new velocity = " + velocity.magnitude);
        }
    }
}