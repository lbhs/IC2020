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

    // Start is called before the first frame update
    private void Start()
    {
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

        StartCoroutine(BlinkScoreDifference());
    }
    
    private IEnumerator BlinkScoreDifference()
    {
        while (true)
        {
            scoreComparison.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            scoreComparison.gameObject.SetActive(false);
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
}
