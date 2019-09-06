using UnityEngine;
using UnityEngine.UI;

public class MenuVisibilityCtrl : MonoBehaviour
{
    [SerializeField] bool shouldStartVisible = default;

    GameObject _myGameObj;
    GameObject _invisibleBG;

    void Awake()
    {
        SetupInvisibleBG();
        _myGameObj = gameObject;
        if (!shouldStartVisible)
            Hide();
    }

    void SetupInvisibleBG()
    {
        _invisibleBG = new GameObject("InvisibleBG");

        InvisibleBGCtrl tempInvisibleBGCtrl = _invisibleBG.AddComponent<InvisibleBGCtrl>();
        tempInvisibleBGCtrl.SetParentCtrl(this);

        Image tempImage = _invisibleBG.AddComponent<Image>();
        tempImage.color = new Color(1f, 1f, 1f, 0f);

        RectTransform tempTransform = _invisibleBG.GetComponent<RectTransform>();
        tempTransform.anchorMin = new Vector2(0f, 0f);
        tempTransform.anchorMax = new Vector2(1f, 1f);
        tempTransform.offsetMin = new Vector2(0f, 0f);
        tempTransform.offsetMax = new Vector2(0f, 0f);
        tempTransform.SetParent(GetComponentsInParent<Transform>()[1], false);
        tempTransform.SetSiblingIndex(transform.GetSiblingIndex()); // put it right beind this panel in the hierarchy
        if (gameObject.name == "WildCardMenu")
        {
            tempTransform.offsetMin = new Vector2(150f, 0);
        }

    }

    void OnEnable()
    {
        _invisibleBG.SetActive(true);
    }

    public void Hide()
    {
        _myGameObj.SetActive(false);
        _invisibleBG.SetActive(false);
    }
}