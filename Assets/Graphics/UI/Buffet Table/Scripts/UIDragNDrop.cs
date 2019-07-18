/*
- TODO:
- Document this.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float mass;
    public int charge;
    public bool elastic;
    public Color color;
    public float scale;
    private int buffetNum;

    public bool UsingMe;
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        UsingMe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        UsingMe = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        buffetNum = int.Parse(gameObject.name);
    }
}
