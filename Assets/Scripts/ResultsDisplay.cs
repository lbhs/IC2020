using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ResultsDisplay : MonoBehaviour
{
    [SerializeField]
    private Text won;

    [SerializeField]
    private Text lost;

    [SerializeField]
    private Text tied;

    [SerializeField]
    private Text scoreComparison;

    private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSe4SJBjv-A6yLFS-dGUHRmd_bVUj_JT95OlOMwgGFmXcZhPtQ/formResponse";

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Post(GameEndInfo.Player1Name, GameEndInfo.Player2Name, GameEndInfo.Player1Score, GameEndInfo.Player2Score));

        if (GameEndInfo.Player1Score > GameEndInfo.Player2Score)
        {
            if (GameEndInfo.LocalPlayer == PhotonNetwork.PlayerList[0])
            {
                won.gameObject.SetActive(true);
            }
            else
            {
                lost.gameObject.SetActive(true);
            }
            scoreComparison.text = string.Format("By {0} Points!", GameEndInfo.Player1Score - GameEndInfo.Player2Score);
        }
        else if (GameEndInfo.Player1Score == GameEndInfo.Player2Score)
        {
            tied.gameObject.SetActive(true);
        }
        else
        {
            if (GameEndInfo.LocalPlayer == PhotonNetwork.PlayerList[0])
            {
                lost.gameObject.SetActive(true);
            }
            else
            {
                won.gameObject.SetActive(true);
            }
            scoreComparison.text = string.Format("By {0} Points!", GameEndInfo.Player2Score - GameEndInfo.Player1Score);
        }
    }

    public void RestartGame()
    {
        PhotonNetwork.Disconnect();
        CleanGameBeforeRestart();
        SceneManager.LoadSceneAsync(0);
    }

    private void CleanGameBeforeRestart()
    {
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("DontDestroyOnLoad"))
        {
            Destroy(GO);
        }
    }

    private IEnumerator Post(string player1Name, string player2Name, int player1Score, int player2Score)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.951314203", player1Name);
        form.AddField("entry.520589148", player2Name);
        form.AddField("entry.1175776431", player1Score);
        form.AddField("entry.1666552715", player2Score);
        byte[] data = form.data;

        WWW www = new WWW(BASE_URL, data);
        yield return www;
    }
}
