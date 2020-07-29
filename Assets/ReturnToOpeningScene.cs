using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToOpeningScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpeningSceneLoader()
    {
        ButtonsDelayedAppearance.frameMarker = 160;
        SceneManager.LoadScene(0);
    }
}
