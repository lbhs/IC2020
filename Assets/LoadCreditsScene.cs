using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCreditsScene : MonoBehaviour
{

    public void CreditsSceneLoader()
    {
        SceneManager.LoadScene(4);
    }

    public void ReturnToCallerScene()
    {
        SceneManager.LoadScene(0);
    }
}
