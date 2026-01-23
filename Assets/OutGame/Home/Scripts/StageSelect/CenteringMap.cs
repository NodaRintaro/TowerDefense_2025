using System;
using UnityEngine;

public class CenteringMap : MonoBehaviour
{
    RectTransform _rectTransform;
    RectTransform _parentRectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void Centering()
    {
        _parentRectTransform.localScale = Vector3.one;
        _parentRectTransform.anchoredPosition = new Vector3(-_rectTransform.localPosition.x, -_rectTransform.localPosition.y-150, 0);
    }
}