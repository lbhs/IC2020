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
    public bool pullBack;


    // Start is called before the first frame update
    void Start()
    {
        Plunger = gameObject.GetComponent<Rigidbody>();
        pressureSlider = GameObject.Find("pressureSlider").GetComponent<Slider>();
        Clamp = false;    //plunger is free to move
        //print(Clamp);
        Atm = pressureSlider.value;
        //print(Atm);
        sliderForce = new Vector3(200f*(Atm-1),0,0);
        
    }

    private void ReintroduceParticles()
    {
        foreach (Collider obj in Physics.OverlapSphere(new Vector3(2, 0, 0), 300))
        {
            if ((obj.gameObject.GetComponent<O2GasScript>() != null || obj.gameObject.GetComponent<GasMoleculeMotion>() != null) && obj.gameObject.transform.position.x > 14f && Time.frameCount % 10 == 0 && obj.GetComponent<Rigidbody>().velocity.magnitude < 20f)
            {
                obj.gameObject.transform.position = new Vector3(20, 0, 0);
                obj.GetComponent<Rigidbody>().velocity = new Vector3(-50f, Random.Range(-0.2f, 0.2f), 0);
                break;
            }
        }
    }

    void FixedUpdate()
    {
        if (pullBack)
        {
            ReintroduceParticles();
        }
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
        
        //print("Atm" + Atm);
        //sliderForce = new Vector3(200f*(Atm-1),0,0);
        Plunger.AddForce(sliderForce);
        //print("sliderForce" + sliderForce);
        //print(Plunger.velocity.x);
        if (pullBack)
        {
            if(Plunger.position.x > 0f)
            {
                Plunger.AddForce(new Vector3(-500f, 0, 0));
            } else
            {
                Plunger.isKinematic = true;
                Plunger.isKinematic = false;
                pullBack = false;
            }
            
        }
        else if (Plunger.position.x >= 12.975)
        {
            if (Atm > 1)
            {
                Plunger.isKinematic = true;
            }
            else
            {
                if (GameObject.Find("holeText").GetComponent<editText>().toggle)
                {
                    Plunger.isKinematic = false;
                    pullBack = true;
                }
            }
        }
        else if ((GameObject.Find("holeText").GetComponent<editText>().toggle && Atm == 1) | (!GameObject.Find("holeText").GetComponent<editText>().toggle))
        {
            if ((Plunger.position.x >= 13.5 - 14 / Atm) && (Plunger.position.x <= 14.5 - 14 / Atm))
            {
                Clamp = true;
                //print("yes");
            }

            if (Clamp == true)
            {
                //print("yes2");
                PlungerClamp = Mathf.Clamp(Plunger.position.x, (14f - 14f / Atm - 0.5f), 14f - 14f / Atm + 0.5f);
                //print(Clamp);

                //print ("PlungerClamp value" + PlungerClamp);
                //print ("sliderForce value" + sliderForce);
                Plunger.position = new Vector3(PlungerClamp, 0, 0);

                if (Plunger.position.x < 14 - 14f / Atm)
                {
                    sliderForce = new Vector3(500f * (Atm - 1), 0, 0);
                    //print("augmentedsliderForce" + sliderForce);
                }
                else
                {
                    sliderForce = new Vector3(250f * (Atm - 1), 0, 0);
                    //print("normal force"+sliderForce);
                }
            }
        }
        else if (GameObject.Find("holeText").GetComponent<editText>().toggle)
        {
            Clamp = false;
            print("success");
            sliderForce = new Vector3(700f * (Atm - 1), 0, 0);
        }
    }
}
/*while (Plunger.position.x > 0f)
                    {
                        Plunger.AddForce(new Vector3(-500f,0,0));
                    }
                    Plunger.isKinematic = true;
                    Plunger.isKinematic = false;
*/
