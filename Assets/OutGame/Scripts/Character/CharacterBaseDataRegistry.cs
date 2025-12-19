using CharacterData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのデータをまとめて管理するScriptableObjectClass
/// </summary>
[CreateAssetMenu(fileName = "CharacterDataList", menuName = "ScriptableObject/CharacterDataList")]
public class CharacterBaseDataRegistry : MasterDataBase<CharacterBaseData>
{
    public CharacterBaseData GetData(uint id)
    {
        foreach (var item in _dataHolder)
        {
            if (item.CharacterID == id)
            {
                return item;
            }
        }
        return null;
    }
}
