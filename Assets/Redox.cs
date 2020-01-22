using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Redox : MonoBehaviour
{
    private forces mainObject;
    private float probability;
    private float tempfactor;
    private Slider temperatureSlider;
    public AudioSource Soundsource;
    public AudioClip Playthis;
    
    
    [Header("Choose One (choosing none will make this a spectator ion)")]
    public bool isReducingAgent;
    public bool isOxidizingAgent;

    [Rename("Electrode Potential Eº (Volts)")]
    public float EP;

    [Header("This is the particle that should replace the current one when the reaction occurs")]
    public GameObject ReactionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mainObject = GameObject.Find("GameObject").GetComponent<forces>();
        temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
        Soundsource = GameObject.Find("Sounds").GetComponent<AudioSource>();
        //Playthis = GameObject.Find("Sounds").GetComponent<AudioClip>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Redox>() != null)
        {
            Redox otherP = collision.gameObject.GetComponent<Redox>(); //otherP stands for other particle
            if (otherP.isReducingAgent == true && isOxidizingAgent == true)
            {
                //temp = temperatureSlider.value;
                tempfactor = 5.1f/temperatureSlider.value; 
                probability = Random.Range(0.0f,tempfactor);
                print(probability);
                if (probability < EP + otherP.EP)
                {
                    
                    //gets positions of both objects
                    Vector3 Rpos = gameObject.transform.position;
                    Vector3 Opos = otherP.transform.position;

                    //spawn the new objects with the old coordinates but flipped
                    Instantiate(otherP.ReactionPrefab, Opos, Quaternion.identity);
                    Instantiate(ReactionPrefab, Rpos, Quaternion.identity);

                    //"destroy" the old objects
                    otherP.transform.position = new Vector3(-15, 0, -15);
					otherP.GetComponent<Rigidbody>().isKinematic = true;
					transform.position = new Vector3(-15, 0, -15);
					GetComponent<Rigidbody>().isKinematic = true;
					mainObject.gameObjects.Remove(gameObject);
					
                    Soundsource.Play();

                    //The need to rename the gameobject is so that it loses the [P] tag
                    //The tag will automatically re-add the particle to the physics list
                    //If an object is destroyed without being removed from the physics list,
                    //all physics will stop until it is resolved
                    
                }
            }
        }
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		if(GetComponent<TimeBody>().frame == -1)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			mainObject.gameObjects.Add(gameObject);
		}
	}
}