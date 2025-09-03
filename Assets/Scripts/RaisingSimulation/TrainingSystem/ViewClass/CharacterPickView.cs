using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterPickView : UIViewBase
{
    [SerializeField, Header("characterの選択ボタンをきれいにまとめるためのコンポーネント")] 
    GridLayoutGroup _characterButtonGrid;

    [SerializeField, Header("characterの選択ボタン")] 
    Button[] _characterButtons;

    
}
