using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SupportCardSelectView
{
    public class ViewData : IViewData
    {
        [SerializeField]private GameObject _canvasObj;



        public GameObject CanvasObj => _canvasObj;
    }

    [Serializable]
    public class CardSelectButton
    {
        [SerializeField] private uint _buttonDeckNum;

        [SerializeField] private Button _button;

        [SerializeField] private Image _image;

        public uint ButtonDeckNum => _buttonDeckNum;
        public Button Button => _button;
        public Image Image => _image;

        public void SetButtonData(Button button, Image image, uint id)
        {
            _button = button;
            _image = image;
        }
    }

    [Serializable]
    public struct ParameterText
    {
        [SerializeField] private TMP_Text _statusText;

        [SerializeField] private ParameterType _statusType;

        public TMP_Text TMP_StatusText => _statusText;
        public ParameterType StatusType => _statusType;

        public void SetStatusText(string text)
        {
            _statusText.text = text;
            _statusText.color = Color.black;
        }
    }

    [Serializable]
    public struct ParameterSlider
    {
        [SerializeField] private Slider _slider;

        [SerializeField] private ParameterType _statusType;

        public ParameterType StatusType => _statusType;

        public void SetSlider(uint maxValue, uint value)
        {
            _slider.maxValue = maxValue;
            _slider.value = value;
        }
    }
}