using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool returnToZero = false; //default value is false
    public GameObject PrefabToSpawn;
    private Vector3 prefabWorldPosition;

    public bool UseingMe;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        UseingMe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        prefabWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabWorldPosition.z = 0;

        if (AbleToSpawn())
        {
            if (AtomInventory.Instance.PrefabCanBeDrawn(PrefabToSpawn.name, true))
            {
                if (returnToZero == true)
                {
                    transform.localPosition = Vector3.zero;
                }
                UseingMe = false;

                GameObject GO = PhotonNetwork.Instantiate(PrefabToSpawn.name, prefabWorldPosition, Quaternion.identity);

                if (GO.tag == "Diatomic")
                {
                    JouleDisplayController.Instance.GetComponent<PhotonView>().RPC("IncrementJDC", RpcTarget.All, 0, GO.GetComponent<PhotonView>().ViewID, 10);
                }

                GameSetupContrller.Instance.GetComponent<PhotonView>().RPC("CalExit", RpcTarget.All);
            }
            else
            {
                ConversationTextDisplayScript.Instance.OutOfInventory2();
                transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }

    public bool AbleToSpawn()
    {
        // from where the raycast is fired
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                // the roll panel tiles
                if (go.gameObject.transform.parent.gameObject.name == "RollPannelSingle" || go.gameObject.transform.parent.gameObject.name == "RollPannelDouble")
                {
                    return false;
                }
            }
        }

        GameObject dummyObject = Instantiate(PrefabToSpawn, prefabWorldPosition, Quaternion.identity);
        int accuracy = 5; //1 is pixel perfect accuracy but causes stutter, 5 is a great performance but could allow minor overlap
        int range = Screen.height / 2;

        for (int x = (int)Input.mousePosition.x - range; x < (int)Input.mousePosition.x + range; x += accuracy)
        {
            for (int y = (int)Input.mousePosition.y - range; y < (int)Input.mousePosition.y + range; y += accuracy)
            {
                // ScreenPointToRay: ray drawn from the camera's near plane through (x, y, 0)
                // ScreenPointToRay.origin: world space starting position of the ray
                // the rays are drawn in the direction of Vector2.zero, which means all directions (a point)?
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, Vector2.zero);
                //Debug.DrawRay(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, transform.TransformDirection(Vector3.forward) * 100, Color.green, 10f, false);
                if (hits.Length > 1)
                {
                    foreach (var go in hits)
                    {
                        if (go.rigidbody.gameObject == dummyObject)
                        {
                            foreach (var go2 in hits)
                            {
                                // there is stacking of GameObjects if g02 is a valid atom
                                if (go2.rigidbody.gameObject != dummyObject && (go2.rigidbody.gameObject.GetComponent<DragIt>() != null || go2.rigidbody.gameObject.tag == "Diatomic"))
                                {
                                    Destroy(dummyObject);
                                    Debug.DrawRay(Camera.main.ScreenPointToRay(new Vector3(x, y, 0)).origin, transform.TransformDirection(Vector3.forward) * 100, Color.green, 10f, false);
                                    ConversationTextDisplayScript.Instance.noStack();
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
        }
        Destroy(dummyObject);
        return true;
    }
}