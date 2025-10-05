using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterPickUIHolder : ITrainigUiHolder
{
    [SerializeField, Header("Canvasのオブジェクト")]
    GameObject _viewCanvas = null;

    [SerializeField, Header("characterの選択ボタンをまとめる")] 
    GridLayoutGroup _characterButtonGrid = null;

    [SerializeField, Header("characterのステータスをまとめる")]
    GridLayoutGroup _characterStatusGrid = null;

    [SerializeField, Header("characterのステータステキスト")]
    TMP_Text[] _characterStatus = default;

    [SerializeField, Header("characterの選択ボタン")] 
    Button[] _characterButtons = default;

    [SerializeField, Header("キャラクターの立ち絵")]
    Sprite _characterSprite = null;

    public GameObject ViewCanvasObj => _viewCanvas;
    public GridLayoutGroup CharacterButtonGrid => _characterButtonGrid;
    public GridLayoutGroup GridStatusTextGroup => _characterStatusGrid;
    public TMP_Text [] CharacterStatusText => _characterStatus;
    public Button[] CharacterButtons => _characterButtons;
    public Sprite CharacterSprite => _characterSprite;


}
