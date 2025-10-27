using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IView
{
    public GameObject ViewCanvasObj { get; }
}

[Serializable]
public struct StatusText
{
    [SerializeField] private TMP_Text _statusText;

    [SerializeField] private StatusType _statusType;

    public TMP_Text TMP_StatusText => _statusText;
    public StatusType StatusType => _statusType;

    public void SetStatusText(string text)
    {
        _statusText.text = text;
        _statusText.color = Color.black;
    }
}

[Serializable]
public struct StatusSlider 
{ 
    [SerializeField] private Slider _slider;

    [SerializeField] private StatusType _statusType;

    public StatusType StatusType => _statusType;

    public void SetSlider(uint maxValue, uint value)
    {
        _slider.maxValue = maxValue;
        _slider.value = value;
    }
}

public enum StatusType
{
    None,
    ID,
    Name,
    Physical,
    Power,
    Intelligence,
    Speed,
    RoleType
}
