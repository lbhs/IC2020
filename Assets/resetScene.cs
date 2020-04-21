using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class resetScene : MonoBehaviour
{
    public GameObject DisplayCanvas;
    public Font ken;

    public void gameOver()
    {
        DontDestroyOnLoad(DisplayCanvas);
        SceneManager.LoadScene("GameOver");
        DisplayCanvas.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
        DisplayCanvas.transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
        DisplayCanvas.transform.GetChild(2).gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 50, 0);
        DisplayCanvas.transform.GetChild(2).gameObject.GetComponent<UnityEngine.UI.Text>().font = ken;
        DisplayCanvas.transform.GetChild(3).gameObject.transform.position = new Vector3(0, -500, 0);
    }

    public void reset()
    {
        Destroy(GameObject.Find("ScoreDisplay"));
        SceneManager.LoadScene(0);
    }
}

