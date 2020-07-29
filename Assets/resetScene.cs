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
		foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
		    {
			         Destroy(o);
        }
        FinalScore = DisplayCanvasScript.ScoreTotal;
        StartCoroutine(LoadSceneTimer(3));
        //SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);

        Debug.Log(FinalScore);
        //SceneManager.LoadScene("GameOver");  
        //SceneManager.UnloadSceneAsync(0);


    }

    public void reset()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
		{
			Destroy(o);
        }
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
        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Additive);
    }
}
