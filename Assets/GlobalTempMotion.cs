using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalTempMotion : MonoBehaviour
{
    public Vector3 velocity;
	private Rigidbody Molecule;
    public static int waterTemp;
    public static float Joules;
    public Material[] MolcColor;  
    Renderer rend;
    public GameObject jwelPrefab;
    public bool useJewlPrefab = false;
    private List<GameObject> JwelImageObjects = new List<GameObject>();
    

    //temperature changes as reactions occur;

    // Start is called before the first frame update
    void Start()
    {
        waterTemp = 1;
        Molecule = gameObject.GetComponent<Rigidbody>();
		//print("initial waterTemp = " + waterTemp);
        
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = MolcColor[waterTemp];
		
		float vx = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vx) && (vx < 1))
			{ vx = 2;}

		float vy = UnityEngine.Random.Range(-5, 5);
		if ((-1 < vy) && (vy < 1))
			{ vy = 2;}

		velocity = new Vector3(vx, vy, 0);
		Molecule.velocity = velocity.normalized*9*waterTemp;
        

        //waterTemp = static value for all molecules:  values 0-4 are color coded;
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
        //print("waterTemp=" + waterTemp);
        
        rend.sharedMaterial = MolcColor[waterTemp];
        if(useJewlPrefab == true)
        {
            JewelChange(waterTemp);
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
        
        if(Molecule.velocity.sqrMagnitude<70*waterTemp)
        {
            //print ("Water speed boost to Temp" + waterTemp);
            Molecule.velocity *= 1.2f;
        }
        else if(Molecule.velocity.sqrMagnitude>90*waterTemp)
        {
            //print ("Water speed reduced to Temp" + waterTemp);
            Molecule.velocity /= 1.1f;
        }
    }

    private void JewelChange(int waterTemp)
    {
        if(waterTemp == 0)
        {
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
        }
        if(waterTemp == 1)
        {
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
        }
        if(waterTemp == 2){
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
            JwelImageObjects[1].gameObject.SetActive(true);
        }
        if(waterTemp == 3){
            foreach (var item in JwelImageObjects)
            {
                item.gameObject.SetActive(false);
            }
            JwelImageObjects[0].gameObject.SetActive(true);
            JwelImageObjects[1].gameObject.SetActive(true);
            JwelImageObjects[2].gameObject.SetActive(true);
        }
        if(waterTemp == 4){
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
