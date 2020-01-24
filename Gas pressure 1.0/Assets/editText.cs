using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editText : MonoBehaviour
{
    public bool toggle = false;
    public void onClick()
	{
        toggle = !toggle;
        if(toggle)
		{
            GetComponent<UnityEngine.UI.Text>().text = "Close End";
            GameObject.Find("right wall (1)").GetComponent<Transform>().transform.position = new Vector3(14, 200, 0);

        } else
		{
            GetComponent<UnityEngine.UI.Text>().text = "Open End";
            GameObject.Find("right wall (1)").GetComponent<Transform>().transform.position = new Vector3(14, 0, 0);
        }
	}
}
