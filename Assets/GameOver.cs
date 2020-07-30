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
  public AudioSource MedalAwardSound;
  public AudioSource CountUpSound;
  public AudioSource CopperSound;
  public AudioSource SilverSound;
  public AudioSource GoldSound;
  public AudioSource PlatSound;
  public AudioSource ApplauseSound;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("New Scene Started");
        FinalGameOverScore = resetScene.FinalScore; //
        FinalScoreText.text = "You got ...";
        Debug.Log("Final score is " + FinalGameOverScore);
        MedalAwardSound = GameObject.Find("MedalAwardSource").GetComponent<AudioSource>();
        CountUpSound = GameObject.Find("CountUpSource").GetComponent<AudioSource>();
        CopperSound = GameObject.Find("CopperSource").GetComponent<AudioSource>();
        SilverSound = GameObject.Find("SilverSource").GetComponent<AudioSource>();
        GoldSound = GameObject.Find("GoldSource").GetComponent<AudioSource>();
        PlatSound = GameObject.Find("PlatSource").GetComponent<AudioSource>();
        ApplauseSound = GameObject.Find("ApplauseSource").GetComponent<AudioSource>();
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
        if(FinalGameOverScore <= 0)
        {
            FinalGameOverScore = 5;
        }

      yield return new WaitForSeconds(0.5f);
      FinalScoreText.text = (" ");
      yield return new WaitForSeconds(0.5f);
      FinalScoreText.text = ("You Got ...");
      yield return new WaitForSeconds(0.5f);
      FinalScoreText.text = (" ");
      yield return new WaitForSeconds(0.5f);
      CountUpSound.Play();
      CountUpSound.loop = true;

      StartCoroutine(CountUpToTarget());
    }


    IEnumerator CountUpToTarget()
    {
        while (CurrentDisplayScore < FinalGameOverScore)
        {
            CurrentDisplayScore += 5;
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
      yield return new WaitForSeconds(1.25f);
      Instantiate (CopperMedal, new Vector3( 4.87f, 1.1f, 0f), Quaternion.identity);
      MedalAwardSound.Play();
      /*for (float i = 0f; i <=1f ; i += .01f)
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
      Debug.Log(TestColor.a);*/

    }
    IEnumerator InstantiateSilver()
    {
      yield return new WaitForSeconds(1.25f);
      Instantiate(SilverMedal, new Vector3( 5.95f, 1.19f, 0f), Quaternion.identity);
      MedalAwardSound.Play();
      /*SpriteRenderer SilverRenderer = SilverMedal.GetComponent<SpriteRenderer>();    //more fade script stuff that doesn't work
      for (float i = 0; i <= 1; i+= 0.01f)
      {
        SilverRenderer.color = new Color (1, 1, 1, i);

      }*/
    }


    IEnumerator InstantiateGold()
    {
      yield return new WaitForSeconds (1.25f);
      Instantiate(GoldMedal, new Vector3(4.65f, 1.51f, 0f), Quaternion.identity);
      MedalAwardSound.Play();
    }
    IEnumerator InstantiatePlat()
    {
      yield return new WaitForSeconds(1.25f);
      Instantiate(PlatMedal, new Vector3(7.44f, 0.51f, 0f), Quaternion.identity);
      MedalAwardSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
      if(CurrentDisplayScore == FinalGameOverScore)
      {
        if (PointCountUpConcluded == false)
        {
          CountUpSound.Stop();


          StartCoroutine(BlinkScore());
          if (FinalGameOverScore < 300)
            {
              /*Instantiate (CopperMedal, new Vector3( 4.87f, 1.1f, 0f), Quaternion.identity);   //fade script that doesn't work
              Debug.Log("Copper Medal Instantiated");
              SpriteRenderer testCopperMedal = CopperMedal.GetComponent<SpriteRenderer>();
              var TestColor = testCopperMedal.color;
              Debug.Log(TestColor.a);*/
              CopperSound.Play();
              StartCoroutine(InstantiateCopper());
              //Debug.Log("Coroutine initiated and ended?");

              PointCountUpConcluded = true;
            }
          else if (FinalGameOverScore >= 300 && FinalGameOverScore < 350)
            {
              SilverSound.Play();
                StartCoroutine(InstantiateSilver());
                PointCountUpConcluded = true;
            }
          else if (FinalGameOverScore >= 350 && FinalGameOverScore < 380)
            {
              GoldSound.Play();
              StartCoroutine(InstantiateGold());
              PointCountUpConcluded = true;
            }
          else if (FinalGameOverScore >= 380)
            {
              ApplauseSound.Play();
              PlatSound.Play();
              StartCoroutine(InstantiatePlat());
              PointCountUpConcluded = true;
            }

        }
      }

    }


}
