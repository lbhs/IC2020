using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public void alterArrow (float pressure) {
        if (pressure == 2.0f)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        } else if (pressure == 3.0f) {
            this.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        } else {
            this.transform.localScale = new Vector3(0, 1, 1);
        }
    }
}
