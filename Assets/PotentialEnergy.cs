using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotentialEnergy : MonoBehaviour
{
  
	private GameObject AtomInQuestion;  //used to be Rigidbody2D
    public float PE;
    private int i;
    //public GameObject PEJewelDisplay;
    public bool useJewelPrefab = false;
    private List<GameObject> JewelImageObjects = new List<GameObject>();
    private Quaternion Rotation;
    private float xOffset;  //Used to store position of the half-joule that needs to be moved due to rotation
    
   
    // Start is called before the first frame update
    void Start()
    {
        AtomInQuestion = gameObject;  //.GetComponent<Rigidbody2D>();
		
               
        if(useJewelPrefab == true)
        {
            foreach (Transform child in AtomInQuestion.transform)
            {
                if(child.tag == "PEJewel")
                {
                    JewelImageObjects.Add(child.gameObject);
                }
                
            }
        }

        Rotation = gameObject.transform.rotation;
        //print("rotation = " + Rotation.eulerAngles.z);
        if (Rotation.eulerAngles.z != 0)  //correct PE Jewel orientation for atoms initially "upsidedown" 
        {
            for (i=0; i < JewelImageObjects.Count; i++)
            {
                JewelImageObjects[i].transform.Rotate(0, 0, -Rotation.eulerAngles.z);  //undoes the THE z-component of parent Euler Angles!
                //print("rotated an initial PE Jewel on " + gameObject);
            }
            
        }

        DisplayPEJoules(PE);   //PE Joules are displayed according to the PE value of this atom
    }

    // Update is called once per frame
    private void Update()
    {    }
       
          
    public void PotentialEnergyAdjust()
    {
        if (useJewelPrefab == true)
        {
            if (PE <0) { PE = 0; }
            DisplayPEJoules(PE);
        }
    }

    public void PotentialEnergyJewelRotation()
    {
        for (i = 0; i < JewelImageObjects.Count; i++)
        {
            JewelImageObjects[i].transform.Rotate(0, 0, -90);  //undoes the THE z-component of parent's Rotation upon Right Click
        }
    }

    public void DisplayPEJoules(float PE)
    {
        
        foreach (var item in JewelImageObjects)
        {
            item.gameObject.SetActive(false);
        }

        if (PE > 0)
        {
            for (i=0; i < 2*PE;i++)
            {
                JewelImageObjects[i].gameObject.SetActive(true);
                
            }
        }

        
    }
}
