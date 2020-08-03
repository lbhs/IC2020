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
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);

        Debug.Log(FinalScore);
        SceneManager.LoadScene("GameOver");  
        //SceneManager.UnloadSceneAsync(0);


    }

    public void reset()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
		{
			Destroy(o);
        }
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
		//SceneManager.UnloadSceneAsync(1);
    }
}
