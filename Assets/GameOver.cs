using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
  public int FinalGameOverScore;
  public Text FinalScoreText;
  public int CurrentDisplayScore = 0;
  public GameObject CopperMedal;
  public GameObject SilverMedal;
  public GameObject GoldMedal;
  public GameObject PlatMedal;
  public bool PointCountUpConcluded = false;
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
      FinalScoreText.text = (" ");
      yield return new WaitForSeconds(0.5f);
      FinalScoreText.text = ("You Got ...");
      yield return new WaitForSeconds(0.5f);
      FinalScoreText.text = (" ");
      yield return new WaitForSeconds(0.5f);

      StartCoroutine(CountUpToTarget());
    }


    IEnumerator CountUpToTarget()
    {
        while (CurrentDisplayScore < FinalGameOverScore)
        {
            CurrentDisplayScore += 1;
            CurrentDisplayScore = Mathf.Clamp(CurrentDisplayScore, 0, FinalGameOverScore);
            FinalScoreText.text = CurrentDisplayScore + " points!";
            yield return null;
        }
    }

    IEnumerator BlinkScore()
    {
      while(true) //infinite loop
      {
        FinalScoreText.text = FinalGameOverScore + " points!";
        yield return new WaitForSeconds (0.5f);
        FinalScoreText.text = " ";
        yield return new WaitForSeconds (0.5f);


      }
    }

    IEnumerator InstantiateCopper()            //Can't get the fade in to work, but since it doesn't do anything I'm leaving the code here. Would be fantastic if someone figured out how to make it work!
    {
      for (float i = 0f; i <=1f ; i += .01f)
      {
        SpriteRenderer CopperRenderer = CopperMedal.GetComponent<SpriteRenderer>();
        Color CopperColor = CopperRenderer.color;
        CopperColor.a = i;
        CopperRenderer.color = CopperColor;
        Debug.Log(CopperColor.a);
        yield return new WaitForSeconds (0.025f);

      }
      Debug.Log("Copper Medal faded in");
      SpriteRenderer testCopperMedal = CopperMedal.GetComponent<SpriteRenderer>();
      var TestColor = testCopperMedal.color;
      Debug.Log(TestColor.a);

    }
    IEnumerator InstantiateSilver()
    {
      yield return new WaitForSeconds (0.75f);
      Instantiate(SilverMedal, new Vector3( 4.58f, 1.09f, 0f), Quaternion.identity);
    }
    IEnumerator InstantiateGold()
    {
      yield return new WaitForSeconds (0.75f);
      Instantiate(GoldMedal, new Vector3(3.49f, 1.1f, 0f), Quaternion.identity);
    }
    IEnumerator InstantiatePlat()
    {
      yield return new WaitForSeconds(0.75f);
      Instantiate(PlatMedal, new Vector3(5.14f, 0.47f, 0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
      if(CurrentDisplayScore == FinalGameOverScore)
      {
        if (PointCountUpConcluded == false)
        {


          StartCoroutine(BlinkScore());
          if (FinalGameOverScore < 300)
            {
              Instantiate (CopperMedal, new Vector3( 3.69f, 1.13f, 0f), Quaternion.identity);
              Debug.Log("Copper Medal Instantiated");
              SpriteRenderer testCopperMedal = CopperMedal.GetComponent<SpriteRenderer>();
              var TestColor = testCopperMedal.color;
              Debug.Log(TestColor.a);
              StartCoroutine(InstantiateCopper());
              Debug.Log("Coroutine initiated and ended?");

              PointCountUpConcluded = true;
            }
          else if (FinalGameOverScore >= 300 && FinalGameOverScore < 350)
            {
                StartCoroutine(InstantiateSilver());
                PointCountUpConcluded = true;
            }
          else if (FinalGameOverScore >= 350 && FinalGameOverScore < 380)
            {
              StartCoroutine(InstantiateGold());
              PointCountUpConcluded = true;
            }
          else if (FinalGameOverScore >= 380)
            {
              StartCoroutine(InstantiatePlat());
              PointCountUpConcluded = true;
            }

        }
      }

    }


}
