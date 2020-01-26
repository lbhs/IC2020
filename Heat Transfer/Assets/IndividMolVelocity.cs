using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividMolVelocity : MonoBehaviour
{
    public Vector3 velocity;
	private Rigidbody Molecule;
    public int temp;
    public Material[] MolcColor;  
    Renderer rend;
    //private Slider temperatureSlider;

    // Start is called before the first frame update
    void Start()
    {
        Molecule = gameObject.GetComponent<Rigidbody>();
		//temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
        //temp = temperatureSlider.value;
        print ("water temp" + temp);

        rend = GetComponent<Renderer>();
        rend.sharedMaterial = MolcColor[temp];
		
		float vx = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vx) && (vx < 1))
			{ vx = 2;}

		float vy = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vy) && (vy < 1))
			{ vy = 2;}

		velocity = new Vector3(vx, vy, 0);
		Molecule.velocity = velocity.normalized*5*temp;
        //temp = individual value for each molecule:  values 0-4 are color coded;
    }

    // Update is called once per frame
    void Update()
    {
        //print("temp=" + temp);
        rend.sharedMaterial = MolcColor[temp];
        
        if(Molecule.velocity.sqrMagnitude<70*temp)
        {
            print ("Water speed boost to Temp" + temp);
            Molecule.velocity *= 1.3f;
        }

    }
}
