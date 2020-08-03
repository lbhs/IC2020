using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject LeaderboardDisplay;

    [SerializeField]
    private GameObject StreakDisplay;

    private bool LeaderboardDisplaying = true;

    [SerializeField]
    private DetermineWinStreak streakController;

    public void OnToggleStateChanged()
    {
        LeaderboardDisplaying = !LeaderboardDisplaying;
        if (LeaderboardDisplaying)
        {
            LeaderboardDisplay.SetActive(true);
            StreakDisplay.SetActive(false);
        }
        else
        {
            StreakDisplay.SetActive(true);
            StartCoroutine(streakController.GetDataFromURL());
            LeaderboardDisplay.SetActive(false);
        }
    }
}
