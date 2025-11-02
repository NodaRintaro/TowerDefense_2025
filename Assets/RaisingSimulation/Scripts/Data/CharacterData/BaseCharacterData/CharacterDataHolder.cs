using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterDataList",menuName = "ScriptableObject/CharacterDataList")]
public class CharacterDataHolder : ScriptableObject
{
    [SerializeField,Header("キャラクターのデータリスト")]
    private List<CharacterBaseData> _dataList = new();
    
    public List<CharacterBaseData> DataList => _dataList;

    public void AddData(CharacterBaseData characterData)
    {
        _dataList.Add(characterData);
    }
}