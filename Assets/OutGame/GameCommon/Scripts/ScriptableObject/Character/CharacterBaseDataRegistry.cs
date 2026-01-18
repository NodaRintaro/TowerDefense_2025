using UnityEngine;

/// <summary>
/// キャラクターのデータをまとめて管理するScriptableObjectClass
/// </summary>
[CreateAssetMenu(fileName = "CharacterDataList", menuName = "ScriptableObject/CharacterDataList")]
public class CharacterBaseDataRegistry : DataRegistryBase<CharacterBaseData>
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

    public uint GetID(string name)
    {
        foreach (var item in _dataHolder)
        {
            if (item.CharacterName == name)
            {
                return item.CharacterID;
            }
        }
        return 0;
    }
}
