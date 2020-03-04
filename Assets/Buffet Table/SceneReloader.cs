using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void GoHome()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
        
    }

    public void ReLoadThisScene()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GlobalTempMotion.Joules = 50;
        
    }
}
