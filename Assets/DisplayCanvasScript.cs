using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCanvasScript : MonoBehaviour
{
    public static int JouleTotal;
    public Text jouleText;
    public static int ScoreTotal;
    public static int BonusPointTotal;
    public Text bonusPointText;
    public Text scoreText;
    

    // Start is called before the first frame update
    void Start()
    {
        JouleTotal = 0;
        ScoreTotal = 0;
        BonusPointTotal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        jouleText.text = JouleTotal.ToString() + " Joules of Heat Collected";
        bonusPointText.text = BonusPointTotal.ToString() + " Molecule Bonus Points";
        ScoreTotal = 10 * JouleTotal + BonusPointTotal;
        scoreText.text = ScoreTotal.ToString() + " Total Points";
        
    }
}
