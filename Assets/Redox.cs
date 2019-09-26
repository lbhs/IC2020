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
                    //gets positions of both objects
                    Vector3 Rpos = gameObject.transform.position;
                    Vector3 Opos = otherP.transform.position;

                    //spawn the new objects with the old coordinates but flipped
                    GameObject P = Instantiate(otherP.ReactionPrefab, Opos, Quaternion.identity);
                    GameObject G = Instantiate(ReactionPrefab, Rpos, Quaternion.identity);

                    StartCoroutine(moveToX(G.transform, Opos, P.transform, Rpos, 1.5f, collision));
                    //StartCoroutine(moveToX(P.transform, Rpos, 0.5f));

                    //Destroy the old objects
                    gameObject.name = "destroyed";
                    mainObject.gameObjects.Remove(gameObject);
                    Destroy(otherP.gameObject);

                    otherP.gameObject.name = "destroyed";
                    mainObject.gameObjects.Remove(otherP.gameObject);
                    //Destroy(gameObject);
                    transform.position = new Vector3(100, 100, 100);
                    //The need to rename the gameobject is so that it loses the [P] tag
                    //The tag will automatically re-add the particle to the physics list
                    //If an object is destroyed without being removed from the physics list,
                    //all physics will stop until it is resolved
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

        float counter = 0;

        //Get the current position of the object to be moved
        Vector3 AstartPos = AfromPosition.position;
        Vector3 BstartPos = BfromPosition.position;

        float oldTime = Time.timeScale;
        Time.timeScale = 0;

        zoomIn(collision.contacts[0].point);
        yield return new WaitForSecondsRealtime(0.75f);

        while (counter < duration)
        {
            counter += Time.unscaledDeltaTime;
            AfromPosition.position = Vector3.Lerp(AstartPos, AtoPosition, counter / duration);
            BfromPosition.position = Vector3.Lerp(BstartPos, BtoPosition, counter / duration);
            yield return null;
        }

        zoomOut();

        Time.timeScale = oldTime;

        isMoving = false;
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
