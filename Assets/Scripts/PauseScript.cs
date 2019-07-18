/*
- TODO:
- Document this.
- Fix the Pause / Drag Glitch.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public bool isPaused = false;
    public Sprite pauseSprite;
    public Sprite playSprite;
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
            gameObject.GetComponentInChildren<Image>().sprite = pauseSprite;
        } else {
            Time.timeScale = 0;
            isPaused = true;
            gameObject.GetComponentInChildren<Image>().sprite = playSprite;
        }
    }
}
