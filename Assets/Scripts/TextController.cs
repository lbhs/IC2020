using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    private Text BondScoreText;
    private Text TotalScoreText;
    private Text BonusPointsText;

    public int BondScore;
    public int BonusScore;

    public int PreviousTotalScore;

    // Start is called before the first frame update
    void Start()
    {
        BondScoreText = transform.GetChild(0).GetComponent<Text>();
        TotalScoreText = transform.GetChild(2).GetComponent<Text>();
        BonusPointsText = transform.GetChild(1).GetComponent<Text>();

        BondScore = 0;
        BonusScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PreviousTotalScore != (BondScore * 10 + BonusScore))
        {
            BondScoreText.text = "Heat Collected: " + BondScore * 10;
            BonusPointsText.text = "Molecule Bonus Points: " + BonusScore;
            TotalScoreText.text = "Total Points: " + (BondScore * 10 + BonusScore);
            PreviousTotalScore = BondScore * 10 + BonusScore;
        }
    }
}
