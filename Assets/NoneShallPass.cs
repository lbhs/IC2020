using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneShallPass : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

   
   private void OnCollisionEnter(Collision collider)
   {
       //print(collider.gameObject.tag);
       if(collider.gameObject.tag=="Water" && GlobalTempMotion.waterTemp>3)
       {
           print("this one shall pass");
       }
   }
  

    // Update is called once per frame
    void Update()
    {
        //print("this is it" + GlobalTempMotion.waterTemp);
    }
}
