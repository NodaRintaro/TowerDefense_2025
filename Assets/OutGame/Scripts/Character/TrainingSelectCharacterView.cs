using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 育成ゲームのキャラクター選択画面で選択中のキャラクターのView
/// </summary>
public class TrainingSelectCharacterView : CharacterViewBase
{
    [SerializeField] Image _roleImage;

    [SerializeField] TMP_Text _nameText;

    public void SetRole(Sprite roleSprite)
    {
        _roleImage.sprite = roleSprite;
    }

    public void SetName(string name)
    {
        _nameText.text = name; 
    }
}
