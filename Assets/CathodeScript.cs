using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CathodeScript : MonoBehaviour
{
    public static bool Rxn = false;  //Rxn = true when e-receiver contacts cathode
    public GameObject ReactionPrefab;
    private AudioSource CathodeSound;

    // Start is called before the first frame update
    void Start()
    {
        CathodeSound = GameObject.Find("SoundforCathode").GetComponent<AudioSource>();
    }


    private void OnCollisionEnter(Collision collider)
    {
        //print(collider.gameObject.tag);
        if(collider.gameObject.tag == "e-receiver")
        {
            Rxn = true;
            print("Rxn" + Rxn);
            
            //gets the position of the collider so that it can turn into the reduction product at that point
            Vector3 Rpos = collider.gameObject.transform.position;
            GameObject ReductionProduct;
            
            //spawn the reduction product at this site and move the oxidizing agent to oblivion
            ReductionProduct = Instantiate(ReactionPrefab, Rpos, Quaternion.identity);
            collider.gameObject.transform.position = new Vector3(-15,20,-15);
            collider.gameObject.GetComponent<Rigidbody>().isKinematic=true;

            //play a sound!
            CathodeSound.Play();


        }

    }
  



    // Update is called once per frame
    void Update()
    {
       
    }
}


