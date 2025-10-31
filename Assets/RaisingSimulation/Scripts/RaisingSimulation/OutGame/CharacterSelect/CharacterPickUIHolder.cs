using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterPickUIHolder
{
    [SerializeField, Header("Canvasのオブジェクト")]
    private GameObject _viewCanvas = null;

    [SerializeField, Header("次の表示画面")]
    private GameObject _nextCanvas = null;

    [SerializeField, Header("キャラクターの立ち絵")]
    private Image _characterImage = null;

    [SerializeField, Header("characterのステータステキスト")]
    private ParameterText[] _characterStatusTexts = default;

    [SerializeField, Header("characterのステータスのスライダー")]
    private ParameterSlider[] _characterStatusSliders = default;

    [SerializeField, Header("characterの選択ボタンをまとめる")] 
    private GridLayoutGroup _characterButtonGridLayout = null;

    [SerializeField, Header("キャラクターの決定ボタン")]
    private Button _characterPickButton = null;

    public GameObject ViewCanvasObj => _viewCanvas;
    public GameObject NextCanvasObj => _nextCanvas;
    public GridLayoutGroup CharacterButtonGrid => _characterButtonGridLayout;
    public ParameterText[] CharacterStatusTexts => _characterStatusTexts;
    public Image CharacterSprite => _characterImage;
    public Button CharacterPickButton => _characterPickButton;

    public void SetCharacterSprite(Sprite sprite)
    {
        _characterImage.sprite = sprite;
    }

    public void SetStatusText(ParameterType statusType, string text)
    {
        bool findStatusType = false; 

        foreach (var statusText in _characterStatusTexts)
        {
            if(statusType == statusText.StatusType)
            {
                statusText.SetStatusText(text);
                findStatusType = true;
            }
        }
        
        if (!findStatusType)
        {
            Debug.Log(statusType.ToString());
        }
    }

    public void AddCharacterSelectButton(GameObject buttonObj)
    {
        buttonObj.transform.parent =_characterButtonGridLayout.gameObject.transform;
    }

    public void ChangeButtonCellSize (int size)
    {
        _characterButtonGridLayout.cellSize = new Vector2(size, size);
    }

    public ParameterSlider GetSliderData(ParameterType statusType)
    {
        foreach(var statusSlider in _characterStatusSliders)
        {
            if(statusType == statusSlider.StatusType) { return statusSlider; }
        }

        Debug.Log("見つかりませんでした");
        return default;
    }
}