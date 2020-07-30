using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator FadeAnimator;
    public float FadeTime = 1;
    public void LoadScene(int SceneToLoad)
    {
        StartCoroutine(LoadSceneTimer(SceneToLoad));
        //SceneManager.LoadScene(2);
    }
    IEnumerator LoadSceneTimer(int SceneToLoad)
    {
        FadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(FadeTime);
        SceneManager.LoadScene(SceneToLoad);
    }
}
