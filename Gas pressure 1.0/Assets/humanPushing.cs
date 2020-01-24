using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanPushing : MonoBehaviour
{
    public void alterText(float pressure)
    {
        if (pressure > 1.0f)
        {
            this.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}