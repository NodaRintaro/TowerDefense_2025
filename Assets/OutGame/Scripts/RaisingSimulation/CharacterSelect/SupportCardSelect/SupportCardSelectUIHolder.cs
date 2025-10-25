using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SupportCardSelectUIHolder : IView
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
    private StatusText[] _statusText;

    [SerializeField, Header("トレーニングによるステータスの強化倍率の表示用Slider")]
    private StatusSlider[] _statusSliders;

    [SerializeField, Header("サポートカードの選択ボタンのPrefab")]
    private GameObject _supportCardSelectButtonPrefab;

    private const int _maxSliderValue = 50;

    public GameObject ViewCanvasObj => _canvas;
    public CardSelectButton[] DeckButtonList => _pickCardButtonData;
    public GridLayoutGroup CardSelectButtonsLayout => _cardSelectButtonsLayout;
    public Button BackCharacterSelectButton => _backCharacterSelectButton;
    public Button StartRaisingSiomulationButton => _startRaisingSiomulationButton;
    public StatusSlider[] StatusSlider => _statusSliders;
    public GameObject SupportCardSelectButtonPrefab => _supportCardSelectButtonPrefab;
    public StatusSlider[] StatusSliderList => _statusSliders;

    public StatusSlider GetSliderData(StatusType statusType)
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
                case StatusType.Power:
                    textData.SetStatusText(card.Power.ToString() + "%");
                    GetSliderData(StatusType.Power).SetSlider(_maxSliderValue, card.Power);
                    break;
                case StatusType.Physical:
                    textData.SetStatusText(card.Physical.ToString() + "%");
                    GetSliderData(StatusType.Physical).SetSlider(_maxSliderValue, card.Physical);
                    break;
                case StatusType.Intelligence:
                    textData.SetStatusText(card.Intelligence.ToString() + "%");
                    GetSliderData(StatusType.Intelligence).SetSlider(_maxSliderValue, card.Intelligence);
                    break;
                case StatusType.Speed:
                    textData.SetStatusText(card.Speed.ToString() + "%");
                    GetSliderData(StatusType.Speed).SetSlider(_maxSliderValue, card.Speed);
                    break;
                case StatusType.ID:
                    textData.SetStatusText(card.ID.ToString());
                    break;
                case StatusType.Name:
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
