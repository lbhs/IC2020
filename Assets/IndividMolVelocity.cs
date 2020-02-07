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
    public GameObject jwelPrefab;
    public bool useJewlPrefab = false;
    private List<GameObject> JwelImageObjects = new List<GameObject>();
    

    //private Slider temperatureSlider;

    // Start is called before the first frame update
    void Start()
    {
        Molecule = gameObject.GetComponent<Rigidbody>();
		//temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
        //temp = temperatureSlider.value;
        //print ("water temp" + temp);

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
        if(useJewlPrefab == true)
        {
            foreach (Transform child in jwelPrefab.transform)
            {
                JwelImageObjects.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //print("temp=" + temp);
        rend.sharedMaterial = MolcColor[temp];
        if(useJewlPrefab == true)
        {
            JewelChange(temp);
        }
        if ((Molecule.velocity.x < 3.0f)&&(Molecule.velocity.x > 0))
		{
			//print("particle x velocity low" + Molecule.velocity.x);
			Molecule.AddForce(400,0,0);
		}
        if ((Molecule.velocity.x > -3.0f)&&(Molecule.velocity.x < 0))
		{
			//print("particle x velocity low" + Molecule.velocity.x);
			Molecule.AddForce(-400,0,0);
		}
        if(Molecule.velocity.sqrMagnitude<70*temp)
        {
            print ("Water speed boost to Temp" + temp);
            Molecule.velocity *= 1.2f;
        }
        else if(Molecule.velocity.sqrMagnitude>90*temp)
        {
            print ("Water speed reduced to Temp" + temp);
            Molecule.velocity /= 1.1f;
        }
    }

    private void JewelChange(int temp)
    {
        if(temp == 0)
        {
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
        }
        if(temp == 1)
        {
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
        }
        if(temp == 2){
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
            JwelImageObjects[1].gameObject.SetActive(true);
        }
        if(temp == 3){
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
            JwelImageObjects[1].gameObject.SetActive(true);
            JwelImageObjects[2].gameObject.SetActive(true);
        }
        if(temp == 4){
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
           JwelImageObjects[0].gameObject.SetActive(true);
            JwelImageObjects[1].gameObject.SetActive(true);
            JwelImageObjects[2].gameObject.SetActive(true);
            JwelImageObjects[3].gameObject.SetActive(true);
            //JwelImageObjects[4].gameObject.SetActive(true);

        }
    }
}
