using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
	public static int scoretot;
	public Text scoretxt;
	public static int multitot = 1;
	public Text multitxt;
	public static int hitpoints;
	public Text health;
	public int multime = 120;
	public static int coltot;
	public Text tutorial1;
	public Text tutorial2;
	public Text tutorial3;
	public GameObject glass;
	public invaderMovementRevamped invadermovementrevamped;
	public static int booster = 0;
	public Text boostertest;
    // Start is called before the first frame update
    void Start()
    {
        scoretot = 0;
		hitpoints = 10;
		coltot = 0;
		tutorial1.text = "Press 'Up' to fire a photon";
		tutorial2.text = "Press 'Down' to change photon color";
		tutorial3.text = "Use the left and right keys to move";
		
    }

    // Update is called once per frame
    void Update()
    {
         
		scoretxt.text = "Score: " + scoretot.ToString() + "0";
		multitxt.text = "Score Multiplier: " + multitot.ToString();
		health.text = "Health: " + hitpoints.ToString();
		boostertest.text = " ";
        if(hitpoints == 0)
		{
			print("lose");
			GameObject.Find("LoseText").GetComponent<RectTransform>().localScale = Vector3.one;
            Time.timeScale = 0;
		}
		multime -= 1;
		if(multime == 0)
		{
			multime = 50;
			if(multitot >= 2)
			{
				multitot -= 1;
			}
		if(booster >= 4)
		{
			booster = 0;
			invadermovementrevamped.movespeed();
		}
		
	//collision events
		}
		if(coltot == 1)
		{
			tutorial1.text = "Avoid chlorine atoms and HCl molecules";
			tutorial2.text = " ";
			tutorial3.text = " ";
		}
		if(coltot == 10)
		{
			coltot = coltot + 1;
			Instantiate(glass, new Vector3(-8f, 2f, 0), Quaternion.identity);
			tutorial1.text = "Glass will absorb UV photons";
		}
		if(coltot == 20)
		{
			tutorial1.text = " ";
		}
		if(coltot == 60)
		{
			coltot = coltot + 1;
			Instantiate(glass, new Vector3(-8f, -1f, 0), Quaternion.identity);
		}
		if(coltot == 160)
		{
			coltot = coltot + 1;
			Instantiate(glass, new Vector3(-8f, -4f, 0), Quaternion.identity);
		}
    }
	public void UpdateScore()
	{
		scoretot += 1 * multitot;
		coltot = coltot + 1;
		booster += 1;
	}
	public void MultiUp()
	{
		multitot += 1;
		multime = 100;
	}
	public void MultiDown()
	{
		if(multitot > 4)
		{
		multitot -= 3;
		}
		else 
		{
			multitot = 1;
			
		}
	}
	public void MultiRes()
	{
		if(multitot > 11)
		{
		multitot -= 10;
		}
		else 
		{
			multitot = 1;
			
		}
		hitpoints -=1;
	}
}
