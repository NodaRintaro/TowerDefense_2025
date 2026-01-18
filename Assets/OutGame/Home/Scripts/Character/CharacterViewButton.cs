using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterViewButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] Image _buttonImage;
    [SerializeField] Color _normalColor;
    [SerializeField] Color _highlightColor;
    [SerializeField] Color _pressedColor;
    [SerializeField] Color _selectedColor;
    [SerializeField] Color _disabledColor;
    
    
    public event Action OnClick; 
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
        _buttonImage.color = _pressedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonImage.color = _selectedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonImage.color = _normalColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _buttonImage.color = _normalColor;
    }
}
