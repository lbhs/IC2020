using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AtomController : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private GameObject TurnScreen;

    public List<GameObject> variants = new List<GameObject>();
    private int variantsCounter = 0;


    void Update()
    {
        TurnScreen = GameObject.Find("It's Not Your Turn Screen");
    }

    void OnMouseDown()
    {
        if (TurnScreen == null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 45));
                transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -45);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                if (variantsCounter == 0)
                {
                    if (variantsCounter < variants.Count)
                    {
                        variantsCounter++;
                        variants[0].SetActive(false);
                        variants[1].SetActive(true);
                    }
                    else { return; }
                }
                else if (variantsCounter == 1)
                {
                    if (variantsCounter < variants.Count)
                    {
                        variantsCounter++;
                        variants[1].SetActive(false);
                        variants[2].SetActive(true);
                    }
                    else { return; }
                }

            }
        }
        else
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }



    void OnMouseDrag()
    {
        if (TurnScreen == null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return;
            }
            else
            {
                transform.position = GetMouseAsWorldPoint() + mOffset;
                transform.position = new Vector3(transform.position.z, transform.position.y, 0);
            }
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = 0;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}