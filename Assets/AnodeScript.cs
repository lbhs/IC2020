using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnodeScript : MonoBehaviour
{
    public GameObject OxProd;
	public GameObject electron;
    public GameObject lowestParticle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(CathodeScript.Rxn == true)
       {
           OxidizeOneZinc();
       }
    }

    private void OxidizeOneZinc()
    {
        if(CathodeScript.Rxn == true)
        {
        //GameObject lowestParticle;
        float lowestY = 20;

        foreach(GameObject particle in GameObject.Find("GameObject").GetComponent<forces>().rootObjects)
        {
            if(particle.tag == "e-donor" && particle.transform.position.y <lowestY)
            {
                //find a zinc atom in the anode block
                lowestY = particle.transform.position.y;
                lowestParticle = particle;
                //print("lowestParticle tag"+lowestParticle.tag);
                //print("lowestY" + lowestY);

            }

        }
        //print("oxidize One Zinc");
        CathodeScript.Rxn = false;
        //print(CathodeScript.Rxn);

        GameObject OxidationProduct;
            
        //spawn the reduction product at this site and move the oxidizing agent to oblivion
        OxidationProduct = Instantiate(OxProd, lowestParticle.transform.position, Quaternion.identity);
        lowestParticle.transform.position = new Vector3(-15,20,-15);
        lowestParticle.GetComponent<Rigidbody>().isKinematic=true;
        //print("moved it");
		
		Instantiate(electron, GameObject.Find("ElectronPath").GetComponent<Route>().bezierPosition(0f), Quaternion.identity).GetComponent<BezierFollow>().route = GameObject.Find("ElectronPath");

        
        //replace this zinc atom with a mobile Zn+ ion
        }
        
    }

}
