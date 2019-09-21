using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redox : MonoBehaviour
{
    private forces mainObject;

    [Header("Choose One (choosing none will make this a spectator ion)")]
    public bool isReducingAgent;
    public bool isOxidizingAgent;

    [Rename("Electrode Potential Eº (Volts)")]
    public float EP;

    [Header("This is the particle that should replace the current one when the reaxtion occurs")]
    public GameObject ReactionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mainObject = GameObject.Find("GameObject").GetComponent<forces>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Redox>() != null)
        {
            Redox otherP = collision.gameObject.GetComponent<Redox>(); //otherP stands for other particle
            if (otherP.isReducingAgent == true && isOxidizingAgent == true)
            {
                if (EP + otherP.EP >= 0)
                {
                    //gets positions of both objects
                    Vector3 Rpos = gameObject.transform.position;
                    Vector3 Opos = otherP.transform.position;

                    //spawn the new objects with the old cordanates but fliped
                    Instantiate(otherP.ReactionPrefab, Rpos, Quaternion.identity);
                    Instantiate(ReactionPrefab, Opos, Quaternion.identity);

                    //Destroy the old objects
                    gameObject.name = "destroyed";
                    mainObject.gameObjects.Remove(gameObject);
                    Destroy(otherP.gameObject);

                    otherP.gameObject.name = "destroyed";
                    mainObject.gameObjects.Remove(otherP.gameObject);
                    Destroy(gameObject);
                    //The need to rename the gameobject is so that it looses the [P] tag
                    //The tag will automatilly re-add the particle to the physics list
                    //If an object is destoryed without being removede from the physics list,
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
