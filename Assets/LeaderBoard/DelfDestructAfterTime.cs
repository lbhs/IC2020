using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelfDestructAfterTime : MonoBehaviour
{
    public int seconds;

    void Start()
    {
        StartCoroutine(Destroy(seconds));
    }

    IEnumerator Destroy(int time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }

}
