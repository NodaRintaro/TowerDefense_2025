using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingEventUIHolder : ITrainigUiHolder
{
    [SerializeField, Header("Canvasのオブジェクト")]
    GameObject _viewCanvas;

    [SerializeField, Header("キャラクターのスプライト")]
    Sprite _characterSprite = null;

    [SerializeField, Header("メッセージのテキスト")]
    private TMP_Text _textUi = default;

    public TMP_Text TextUI => _textUi;
    public GameObject ViewCanvasObj => _viewCanvas;
}
