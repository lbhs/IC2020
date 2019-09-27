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

    [Header("This is the particle that should replace the current one when the reaction occurs")]
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
                if (EP + otherP.EP > 0)
                {
                    float chance = (EP + otherP.EP) / 3f;
                    if(chance > Random.Range(0.0f , 1.0f))
                    {
                        //gets positions of both objects
                        Vector3 Rpos = gameObject.transform.position;
                        Vector3 Opos = otherP.transform.position;

                        StartCoroutine(moveToX(gameObject.transform, Opos, otherP.transform, Rpos, 3f, collision));
                        //StartCoroutine(moveToX(P.transform, Rpos, 0.5f));
                    }
                }
            }
        }
    }

    bool isMoving = false;

    IEnumerator moveToX(Transform AfromPosition, Vector3 AtoPosition, Transform BfromPosition, Vector3 BtoPosition, float duration, Collision collision)
    {
        //Make sure there is only one instance of this function running
        if (isMoving)
        {
            yield break; ///exit if this is still running
        }
        isMoving = true;

        float oldTime = Time.timeScale;

        int redoxMax = 2; // the number of times the anamation plays

        if (mainObject.RedoxNumber < redoxMax)
        {
            Time.timeScale = 0;
        }

        float counter = 0;

        //Get the current position of the object to be moved
        Vector3 AstartPos = AfromPosition.position;
        Vector3 BstartPos = BfromPosition.position;

        //float oldTime = Time.timeScale;
        //Time.timeScale = 0;

        if (mainObject.RedoxNumber < redoxMax)
        {
            zoomIn(collision.contacts[0].point);
            yield return new WaitForSecondsRealtime(0.75f);
        }

        while (counter < duration)
        {
            counter += Time.unscaledDeltaTime;
            AfromPosition.position = Vector3.Lerp(AstartPos, AtoPosition, counter / duration);
            BfromPosition.position = Vector3.Lerp(BstartPos, BtoPosition, counter / duration);
            yield return null;
        }

        //color gradient swap
        float Acounter = 0;
        float Aduration = 1;
        Color AStartColor = AfromPosition.gameObject.GetComponent<Renderer>().material.color;
        Color BStartColor = BfromPosition.gameObject.GetComponent<Renderer>().material.color;
        Color AEndColor = AfromPosition.gameObject.GetComponent<Redox>().ReactionPrefab.gameObject.GetComponent<Renderer>().sharedMaterial.color;
        Color BEndColor = BfromPosition.gameObject.GetComponent<Redox>().ReactionPrefab.gameObject.GetComponent<Renderer>().sharedMaterial.color;

        while (Acounter < Aduration)
        {
            Acounter += Time.unscaledDeltaTime;
            AfromPosition.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(AStartColor, AEndColor, Acounter / Aduration);
            BfromPosition.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(BStartColor, BEndColor, Acounter / Aduration);
            yield return null;
        }

        if (mainObject.RedoxNumber < redoxMax)
        {
            //To-Do add other particle flying out
            yield return new WaitForSecondsRealtime(0.75f);
            zoomOut();

            Time.timeScale = oldTime;
        }

        mainObject.RedoxNumber++;

        //actual prefab swap
        finishTheJob(BfromPosition.gameObject.GetComponent<Redox>(), BtoPosition, AtoPosition);

        isMoving = false;

       
    }

    void finishTheJob(Redox otherP, Vector3 Opos, Vector3 Rpos)
    {
        //spawn the new objects with the old coordinates but flipped
        Instantiate(otherP.ReactionPrefab, Opos, Quaternion.identity);
        Instantiate(ReactionPrefab, Rpos, Quaternion.identity);

        //Destroy the old objects
        gameObject.name = "destroyed";
        mainObject.gameObjects.Remove(gameObject);
        Destroy(otherP.gameObject);

        otherP.gameObject.name = "destroyed";
        mainObject.gameObjects.Remove(otherP.gameObject);
        Destroy(gameObject);
        //transform.position = new Vector3(100, 100, 100);

        //The need to rename the gameobject is so that it loses the [P] tag
        //The tag will automatically re-add the particle to the physics list
        //If an object is destroyed without being removed from the physics list,
        //all physics will stop until it is resolved
    }

    private void zoomIn(Vector3 pos)
    {
        Camera.main.orthographicSize = 2;
        Camera.main.transform.position = new Vector3 (pos.x, pos.y, -10);
    }

    private void zoomOut()
    {
        Camera.main.orthographicSize = 8;
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }
}
    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
