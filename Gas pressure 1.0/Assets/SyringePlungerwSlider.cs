using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyringePlungerwSlider : MonoBehaviour
{
    public float Atm;
    private Rigidbody Plunger; 
    private Vector3 sliderForce;
    private float PlungerClamp;
    private bool Clamp;
    private Slider pressureSlider;


    // Start is called before the first frame update
    void Start()
    {
        Plunger = gameObject.GetComponent<Rigidbody>();
        pressureSlider = GameObject.Find("pressureSlider").GetComponent<Slider>();
        Clamp = false;    //plunger is free to move
        print(Clamp);
        Atm = pressureSlider.value;
        print(Atm);
        sliderForce = new Vector3(200f*(Atm-1),0,0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Atm != pressureSlider.value)  //occurs when slider is changed
        {
            Clamp = false;      //plunger is free to move
            Atm = pressureSlider.value;
            sliderForce = new Vector3(250f*(Atm-1),0,0); 
        }
        
        print("Atm" + Atm);
        //sliderForce = new Vector3(200f*(Atm-1),0,0);
        Plunger.AddForce(sliderForce);
        print("sliderForce" + sliderForce);
        //print(Plunger.velocity.x);
        
        if ((Plunger.position.x >= 13.5-14/Atm)&&(Plunger.position.x <= 14.5-14/Atm))
        {
            Clamp = true;
        }
        
        if (Clamp == true)
        {
            PlungerClamp = Mathf.Clamp(Plunger.position.x,(14f-14f/Atm - 0.5f),14f-14f/Atm + 0.5f);
            print(Clamp);
            
            print ("PlungerClamp value" + PlungerClamp);
            print ("sliderForce value" + sliderForce);
            Plunger.position = new Vector3 (PlungerClamp,0,0);
            
            if (Plunger.position.x < 14-14f/Atm)
            {
                sliderForce = new Vector3(500f*(Atm-1),0,0);
                print("augmentedsliderForce" + sliderForce);
            }
            else
            {
                sliderForce = new Vector3(250f*(Atm-1),0,0);
                print("normal force"+sliderForce);
            }
        }

    }
}
