using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyringePlunger : MonoBehaviour
{
    public float Atm;
    private Rigidbody Plunger; 
    private Vector3 sliderForce;
    private float PlungerClamp;
    private bool Clamp;
    private InputField Pressure;
    

    // Start is called before the first frame update
    void Start()
    {
        Plunger = gameObject.GetComponent<Rigidbody>();
        Pressure = GameObject.Find("Atmospheres").GetComponent<InputField>();
        Atm = int.Parse(Pressure.text);
        Clamp = false;
        print(Clamp);
        sliderForce = new Vector3(200f*Atm,0,0);
        //sliderForce *= Atm;
    }

    // Update is called once per frame
    void Update()
    {
        
        Plunger.AddForce(sliderForce);
        //print(sliderForce);
        //print(Plunger.velocity.x);

        if (Plunger.position.x >= 14-(14/Atm))
        {
            Clamp = true;
        }
        
        if (Clamp == true)
        {
            PlungerClamp = Mathf.Clamp(Plunger.position.x,(14f-14f/Atm - 0.5f),14f-14f/Atm + 0.5f);
            print(Clamp);
            
            print (PlungerClamp);
            print (sliderForce);
            Plunger.position = new Vector3 (PlungerClamp,0,0);
            if (Plunger.position.x < 14-14f/Atm)
            {
                sliderForce = new Vector3(500f*Atm,0,0);
                print(sliderForce);
            }
            else
            {
                sliderForce = new Vector3(200f*Atm,0,0);
            }
        }

    }
}
