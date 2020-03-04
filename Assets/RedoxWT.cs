using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedoxWT : MonoBehaviour
{
    private forces mainObject;
    private float probability;
    private float tempfactor;
    public AudioSource Soundsource;
    public AudioClip Playthis;
    
    
    //TO GET THE TEMP SLIDER VERSION OF REDOX, NEED TO GO BACK TO Project ELECTRON TRANSFER 3.4 + HYDROXIDE !!!!!
    
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
        //temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
        Soundsource = GameObject.Find("Sounds").GetComponent<AudioSource>();
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<RedoxWT>() != null)
        {
            RedoxWT otherP = collision.gameObject.GetComponent<RedoxWT>(); //otherP stands for other particle
            if (otherP.isReducingAgent == true && isOxidizingAgent == true)
            {
                //temp = temperatureSlider.value; SLIDER DISABLED in Exothermic rxn

                //Calculates probability of productive collision  for Zn, 0.76/1.46 or about 52% productive
                tempfactor = 5.1f/(GlobalTempMotion.waterTemp+2f); //Normally, denominator = tempSlider value
                probability = Random.Range(0.0f,tempfactor);
                //print("tempfactor" + tempfactor);
                //print("probability " + probability);
                if (probability < EP + otherP.EP)
                {
                    
                    //gets positions of both objects
                    Vector3 Rpos = gameObject.transform.position;
                    Vector3 Opos = otherP.transform.position;

                    GameObject NewObject1;
                    GameObject NewObject2;
                    //spawn the new objects with the old coordinates but flipped
                    NewObject1 = Instantiate(otherP.ReactionPrefab, Opos, Quaternion.identity);
                    NewObject2 = Instantiate(ReactionPrefab, Rpos, Quaternion.identity);

                    //Flag management
                    if (otherP.GetComponent<LabelAssigner>().hasFlag == true)
                    {
                        NewObject2.GetComponent<LabelAssigner>().hasFlag = true;
                    }
                    else if (gameObject.GetComponent<LabelAssigner>().hasFlag == true)
                    {
                        NewObject2.GetComponent<LabelAssigner>().hasFlag = true;
                    }

                    //Destroy the old objects
                    gameObject.name = "destroyed";
                    mainObject.gameObjects.Remove(gameObject);
                    Destroy(otherP.gameObject);

                    otherP.gameObject.name = "destroyed";
                    mainObject.gameObjects.Remove(otherP.gameObject);
                    Destroy(gameObject);

                    //Joules of Heat increases with each reaction  
                    //When Joules reach certain thresholds, waterTemp increases
                    GlobalTempMotion.Joules += ((EP + otherP.EP)*30);
                    print("Joules =" + GlobalTempMotion.Joules);
                    //print("div by 50  " + GlobalTempMotion.Joules/50);
                    GlobalTempMotion.waterTemp = (int)(GlobalTempMotion.Joules/50);
                    if(GlobalTempMotion.waterTemp > 5)
                    {
                        GlobalTempMotion.waterTemp = 5;
                    }

                    //GlobalTempMotion.waterTemp ++;  
                    //waterTemp ++;

                    //Plays a sound
                    Soundsource.Play();

                    //The need to rename the gameobject is so that it loses the [P] tag
                    //The tag will automatically re-add the particle to the physics list
                    //If an object is destroyed without being removed from the physics list,
                    //all physics will stop until it is resolved

                }
            }
        }
    }
}
    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
