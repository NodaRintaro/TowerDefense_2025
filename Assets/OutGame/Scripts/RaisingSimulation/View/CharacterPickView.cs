using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterPickView : IUiHolder
{
    [SerializeField, Header("Canvasのオブジェクト")]
    GameObject _viewCanvas;

    [SerializeField, Header("characterの選択ボタンをきれいにまとめるためのコンポーネント")] 
    GridLayoutGroup _characterButtonGrid;

    [SerializeField, Header("characterの選択ボタン")] 
    Button[] _characterButtons;

    public GameObject ViewCanvasObj => _viewCanvas;
}
