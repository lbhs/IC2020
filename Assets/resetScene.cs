using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class resetScene : MonoBehaviour
{
    public GameObject DisplayCanvas;
    public GameObject ScoreDisplay;
    public Font ken;

    public void gameOver()
    {
        DontDestroyOnLoad(DisplayCanvas);
		foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
		{
			if(!(o == ScoreDisplay || o == DisplayCanvas))
			{
				Destroy(o);
			}
        }
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive); //needed to prevent new scene from loading on the same frame as unloading old scene which crashes webgl
		SceneManager.UnloadSceneAsync(0);
        DisplayCanvas.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
        DisplayCanvas.transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
        DisplayCanvas.transform.GetChild(2).gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 50, 0);
        DisplayCanvas.transform.GetChild(2).gameObject.GetComponent<UnityEngine.UI.Text>().font = ken;
        DisplayCanvas.transform.GetChild(3).gameObject.transform.position = new Vector3(0, -500, 0);
    }

    public void reset()
    {
		foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
		{
			Destroy(o);
        }
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync(1);
    }
}

