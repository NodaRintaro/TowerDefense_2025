using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NovelEventUI
{
    [Header("発言者の名前")]
    [SerializeField] private TMP_Text _speakerNameText;

    [Header("NovelText")]
    [SerializeField] private DialogueText _dialogueText;

    [Header("会話に参加しているキャラクターのイメージ")]
    [SerializeField] private Image _characterCenter;
    [SerializeField] private Image _characterLeftBottom;
    [SerializeField] private Image _characterRightBottom;
    [SerializeField] private Image _characterLeftTop;
    [SerializeField] private Image _characterRightTop;

    [Header("背景")]
    [SerializeField] private Image _backScreen;

    public DialogueText DialogueText => _dialogueText;

    public void SetNameText(string name)
    {
        _speakerNameText.text = name;
    }

    public void SetBackScreen(Sprite backScreen)
    {
        _backScreen.sprite = backScreen;
    }

    public void SetCharacterImage(
        Sprite centerSprite, 
        Sprite leftBottomSprite, 
        Sprite rightBottomSprite, 
        Sprite leftTopSprite, 
        Sprite rightTopSprite)
    {
        _characterCenter.sprite = centerSprite;
        SetImageAlpha(_characterCenter);

        _characterLeftBottom.sprite = leftBottomSprite;
        SetImageAlpha(_characterLeftBottom);

        _characterRightBottom.sprite = rightBottomSprite;
        SetImageAlpha(_characterRightBottom);

        _characterLeftTop.sprite = leftTopSprite;
        SetImageAlpha(_characterLeftTop);

        _characterRightTop.sprite = rightTopSprite;
        SetImageAlpha(_characterRightTop);
    }

    /// <summary> SpriteがNullでなければ表示してNullなら非表示にする処理 </summary>
    public void SetImageAlpha(Image image)
    {
        if (image.sprite != null)
        {
            ShowCharacterImage(image);
        }
        else HideCharacterImage(image);
    }

    private void ShowCharacterImage(Image image)
    {
        Color color = image.color;
        color.a = 255;
        image.color = color;
    }

    private void HideCharacterImage(Image image)
    {
        Color color = image.color;
        color.a = 0;
        image.color = color;
    }
}
