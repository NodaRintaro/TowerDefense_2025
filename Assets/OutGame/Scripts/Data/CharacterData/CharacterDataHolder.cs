using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataList", menuName = "ScriptableObject/CharacterDataList")]
public class CharacterDataHolder : ScriptableObject
{
    [SerializeField, Header("キャラクターのデータリスト")]
    private List<CharacterBaseData> _dataList = new();

    public List<CharacterBaseData> CharacterInformationDataList => _dataList;

    public void AddData(CharacterBaseData characterData)
    {
        _dataList.Add(characterData);
    }

    public CharacterBaseData GetData(uint id)
    {
        foreach (var item in _dataList)
        {
            if (item.CharacterID == id)
            {
                return item;
            }
        }
        return null;
    }
}

