using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JouleHolderScript : MonoBehaviour
{
    public GameObject JUIPrefab;
    public Text JoulesOfHeatInCorral;
    

    public void JSpawn()  //This function is called from the JewelMover script that uses the BondEnergy variable passed from BondMaker script 
    {
        GameObject JouleInCorral = Instantiate(JUIPrefab);
        JouleInCorral.transform.parent = gameObject.transform;
        JouleInCorral.transform.localPosition = new Vector3(Random.Range(-36, 36), Random.Range(-24, 36), 0);
        AdjustJoulesInCorral();
    }

    public void AdjustJoulesInCorral()
    {
        JoulesOfHeatInCorral.text = DisplayCanvasScript.JouleTotal + " Joules of Heat";
    }




    private void FixedUpdate()
    {
        //JoulesOfHeatInCorral.text = DisplayCanvasScript.JouleTotal + " Joules of Heat";
    }

}


