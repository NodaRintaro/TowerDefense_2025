using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterPickView : UIViewBase
{
    [SerializeField, Header("character�̑I���{�^�������ꂢ�ɂ܂Ƃ߂邽�߂̃R���|�[�l���g")] 
    GridLayoutGroup _characterButtonGrid;

    [SerializeField, Header("character�̑I���{�^��")] 
    Button[] _characterButtons;

    
}
