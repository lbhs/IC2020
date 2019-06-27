using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseScript : MonoBehaviour
{
    public bool isPaused = false;
    public void Start()
    {
        pauseGame();
    }
    public void pauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
        } else {
            Time.timeScale = 0;
            isPaused = true; 
        }
    }
}
