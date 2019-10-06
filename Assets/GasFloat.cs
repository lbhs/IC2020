using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasFloat : MonoBehaviour
{
    public Vector3 velocity;
    public float temp;
	private Rigidbody gas;
	private Slider temperatureSlider;
    private GameObject RXNCounterText;

    // Awake is called before the Start function
    private void Awake()
    {
        RXNCounterText = GameObject.Find("ReactionCounter");
    }

    // Start is called before the first frame update
    void Start()
    {
        gas = gameObject.GetComponent<Rigidbody>();
		temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
        gas.velocity = new Vector3(0,4,0);
    }

    private void OnEnable()
    {
        RXNCounterText.GetComponent<reactionCounter>().number++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
