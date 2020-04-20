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
	public GameObject glass;
    // Start is called before the first frame update
    void Start()
    {
        scoretot = 0;
		hitpoints = 10;
		coltot = 0;
    }

    // Update is called once per frame
    void Update()
    {
         
		scoretxt.text = "Score: " + scoretot.ToString();
		multitxt.text = "Score Multiplier: " + multitot.ToString();
		health.text = "Health: " + hitpoints.ToString();
        if(hitpoints == 0)
		{
			print("lose");
			GameObject.Find("LoseText").GetComponent<RectTransform>().localScale = Vector3.one;
            Time.timeScale = 0;
		}
		multime -= 1;
		if(multime == 0)
		{
			multime = 60;
			if(multitot >= 2)
			{
				multitot -= 1;
			}
		}
		if(coltot == 10)
		{
			coltot = coltot + 1;
			Instantiate(glass, new Vector3(-8f, 2f, 0), Quaternion.identity);
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
	}
	public void MultiUp()
	{
		multitot += 1;
		multime = 120;
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
		multitot = 1;
		hitpoints -=1;
	}
}
