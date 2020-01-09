using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydroxideScript : MonoBehaviour
  
{
    public GameObject rxnproduct;
    private forces mainObject;
    private Slider temperatureSlider;
    public AudioSource Soundsource;

    // Start is called before the first frame update
    void Start()
    {
        mainObject = GameObject.Find("GameObject").GetComponent<forces>();
        temperatureSlider = GameObject.Find("temperatureSlider").GetComponent<Slider>();
        Soundsource = GameObject.Find("Sounds").GetComponent<AudioSource>();
        
    }




    private void OnCollisionEnter(Collision collision)
    {
        print (collision.gameObject.name);


        if(collision.gameObject.tag == "H+")
        {
            print (gameObject.name);

            //spawn the new objects with the old coordinates but flipped
                    Instantiate(rxnproduct, transform.position, transform.rotation);
                    

                    //Destroy the old objects
                    //gameObject.name = "destroyed";
                    //mainObject.gameObjects.Remove(gameObject);
                    Destroy(collision.gameObject);

                    //otherP.gameObject.name = "destroyed";
                    //mainObject.gameObjects.Remove(otherP.gameObject);
                    Destroy(gameObject);

                    Soundsource.Play();
        }
           


    }


    // Update is called once per frame
    void Update()
    {
       
    }
}
