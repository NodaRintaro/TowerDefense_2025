using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 純粋なキャラクターのデータ </summary>
[System.Serializable]
public class PureCharacterData : CharacterBaseData
{
    //所持済みの判定
    [SerializeField, Header("所持済みかどうかの判定")]
    protected bool _isGetting = false;

    public bool IsGetting => _isGetting;

    public void GetCharacter() => _isGetting = true;
}