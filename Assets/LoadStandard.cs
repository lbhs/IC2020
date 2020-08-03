using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStandard : MonoBehaviour
{
    public Scene SceneToLoad;

    public void LoadStandardGame()
    {
        SceneManager.LoadScene(2);
    }

}
