using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class resetScene : MonoBehaviour
{
    public GameObject DisplayCanvas;
	public GameObject ScoreDisplay;
    public Font ken;
    public static int FinalScore;

    public void gameOver()
    {
        DontDestroyOnLoad(DisplayCanvas);
        FinalScore = DisplayCanvasScript.ScoreTotal;
        Destroy(DisplayCanvas);
        StartCoroutine(LoadSceneTimer(3));
        //SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);

        Debug.Log(FinalScore);
        //SceneManager.LoadScene("GameOver");  
        //SceneManager.UnloadSceneAsync(0);


    }

    public void reset()
    {
        StartCoroutine(LoadSceneTimer(2));
        //SceneManager.LoadScene(2, LoadSceneMode.Additive);
		//SceneManager.UnloadSceneAsync(1);
    }
    public Animator FadeAnimator;
    public float FadeTime = 1;
    IEnumerator LoadSceneTimer(int SceneToLoad)
    {
        FadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(FadeTime);
        SceneManager.LoadScene(SceneToLoad);
    }
}
