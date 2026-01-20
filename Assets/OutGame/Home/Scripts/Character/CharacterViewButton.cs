using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterViewButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _pressedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _disabledColor;
    
    [SerializeField] private Image _numberImage;
    
    public event Action OnClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
        //_buttonImage.color = _pressedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //_buttonImage.color = _selectedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //_buttonImage.color = _normalColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //_buttonImage.color = _normalColor;
    }
}
