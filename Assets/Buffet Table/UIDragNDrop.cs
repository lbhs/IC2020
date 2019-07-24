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
    private GameObject rightCanvas;

    public bool UseingMe;
    public void OnDrag(PointerEventData eventData)
    {
        rightCanvas.GetComponent<RightClickHelper>().HideRightMenu();
        transform.position = Input.mousePosition;
        UseingMe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        UseingMe = false;
    }
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        rightCanvas = GameObject.Find("Right-Click Canvas");
        num = int.Parse(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
