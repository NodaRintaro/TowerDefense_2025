using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SupportCardDeckView
{
    [SerializeField] Button _mainSupportCardDeckButton;

    [SerializeField] Button[] _subSupportCardDeckButtons;

    [SerializeField] Button[] _currentSelectedButtons = new Button[_cardDeckNum];

    /// <summary> 現在選択中のボタン </summary>
    private Button _currentSelectButton;

    private const int _cardDeckNum = 4;

    /// <summary> 現在選択中のボタン </summary>
    public Button CurrentSelectButton => _currentSelectButton;

    /// <summary> デッキ登録時にそのカードを使えなくする </summary>
    public void SetSelectedButtons(uint deckNum, Button button)
    {
        if(_currentSelectedButtons[deckNum] != null)
            _currentSelectedButtons[deckNum].interactable = true;

        button.interactable = false;
        _currentSelectedButtons[deckNum] = button;
    }

    /// <summary> デッキ登録用のボタンを取得 </summary>
    public Button GetDeckButton(uint supportCardDeckbuttonNum)
    {
        if (supportCardDeckbuttonNum == 0)
        {
            return _mainSupportCardDeckButton;
        }
        else
        {
            //デッキの1番目は_mainSupportCardDeckButtonなのでその分の要素数を"-1"しておく
            uint deckNum = supportCardDeckbuttonNum - 1;
            return _subSupportCardDeckButtons[deckNum];
        }
    }

    /// <summary> デッキ登録用のボタンにサポートカードの画像データをセット </summary>
    public void SetButtonImage(Sprite buttonImage, int supportCardDeckbuttonNum)
    {
        if(supportCardDeckbuttonNum == 0)
        {
            _mainSupportCardDeckButton.image.sprite = buttonImage;
        }
        else
        {
            //デッキの1番目は_mainSupportCardDeckButtonなので要素数を"-1"しておく
            int deckNum = supportCardDeckbuttonNum - 1;
            _subSupportCardDeckButtons[deckNum].image.sprite = buttonImage;
        }
    }
}
