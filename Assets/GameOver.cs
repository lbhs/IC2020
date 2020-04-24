using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
  public int FinalGameOverScore;
  public Text FinalScoreText;
  public int CurrentDisplayScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("New Scene Started");
        FinalGameOverScore = resetScene.FinalScore;
        FinalScoreText.text = "You got ...";
        Debug.Log("Final score is " + FinalGameOverScore);
        StartCoroutine(WaitStart());

    }

/*
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(CountUpToTarget());
    }
*/
    IEnumerator WaitStart()
    {
      yield return new WaitForSeconds(0.5f);

      StartCoroutine(CountUpToTarget());
    }


    IEnumerator CountUpToTarget()
    {
        while (CurrentDisplayScore < FinalGameOverScore)
        {
            CurrentDisplayScore += 1;
            CurrentDisplayScore = Mathf.Clamp(CurrentDisplayScore, 0, FinalGameOverScore);
            FinalScoreText.text = "You got " + CurrentDisplayScore + " points!";
            yield return null;
        }
    }

    IEnumerator BlinkScore()
    {
      while(true) //infinite loop
      {
        FinalScoreText.text = "You got " + FinalGameOverScore + " points!";
        yield return new WaitForSeconds (0.5f);
        FinalScoreText.text = " ";
        yield return new WaitForSeconds (0.5f);

      }
    }

    // Update is called once per frame
    void Update()
    {
      if(CurrentDisplayScore == FinalGameOverScore)
      {
        StartCoroutine(BlinkScore());
      }

    }


}
