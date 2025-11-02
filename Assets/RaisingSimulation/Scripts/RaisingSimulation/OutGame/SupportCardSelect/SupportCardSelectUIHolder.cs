using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SupportCardSelectUIHolder
{
    [SerializeField] private GameObject _canvas;

    [SerializeField, Header("デッキのカードのデータ")] 
    private CardSelectButton[] _pickCardButtonData;

    [SerializeField, Header("カード選択ボタンをまとめるGridLayoutGroup")]
    private GridLayoutGroup _cardSelectButtonsLayout;

    [SerializeField, Header("キャラクターセレクト画面に戻るButton")]
    private Button _backCharacterSelectButton;

    [SerializeField, Header("育成ゲームのスタートボタン")]
    private Button _startRaisingSiomulationButton;

    [SerializeField, Header("ステータスのテキスト")]
    private ParameterText[] _statusText;

    [SerializeField, Header("トレーニングによるステータスの強化倍率の表示用Slider")]
    private ParameterSlider[] _statusSliders;

    [SerializeField, Header("サポートカードの選択ボタンのPrefab")]
    private GameObject _supportCardSelectButtonPrefab;

    private const int _maxSliderValue = 50;

    public GameObject ViewCanvasObj => _canvas;
    public CardSelectButton[] DeckButtonList => _pickCardButtonData;
    public GridLayoutGroup CardSelectButtonsLayout => _cardSelectButtonsLayout;
    public Button BackCharacterSelectButton => _backCharacterSelectButton;
    public Button StartRaisingSiomulationButton => _startRaisingSiomulationButton;
    public ParameterSlider[] StatusSlider => _statusSliders;
    public GameObject SupportCardSelectButtonPrefab => _supportCardSelectButtonPrefab;
    public ParameterSlider[] StatusSliderList => _statusSliders;

    public ParameterSlider GetSliderData(ParameterType statusType)
    {
        foreach (var statusSlider in _statusSliders)
        {
            if (statusType == statusSlider.StatusType) { return statusSlider; }
        }

        Debug.Log("見つかりませんでした");
        return default;
    }

    public void ViewStatus(SupportCardData card)
    {
        foreach (var textData in _statusText)
        {
            switch (textData.StatusType)
            {
                case ParameterType.Power:
                    textData.SetStatusText(card.Power.ToString() + "%");
                    GetSliderData(ParameterType.Power).SetSlider(_maxSliderValue, card.Power);
                    break;
                case ParameterType.Physical:
                    textData.SetStatusText(card.Physical.ToString() + "%");
                    GetSliderData(ParameterType.Physical).SetSlider(_maxSliderValue, card.Physical);
                    break;
                case ParameterType.Intelligence:
                    textData.SetStatusText(card.Intelligence.ToString() + "%");
                    GetSliderData(ParameterType.Intelligence).SetSlider(_maxSliderValue, card.Intelligence);
                    break;
                case ParameterType.Speed:
                    textData.SetStatusText(card.Speed.ToString() + "%");
                    GetSliderData(ParameterType.Speed).SetSlider(_maxSliderValue, card.Speed);
                    break;
                case ParameterType.ID:
                    textData.SetStatusText(card.ID.ToString());
                    break;
                case ParameterType.Name:
                    textData.SetStatusText(card.CardName);
                    break;
            }
        }
    }
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