using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "CharacterResource", menuName = "ScriptableObject/CharacterResource")]
public class CharacterResourceHolder : ScriptableObject
{
    [SerializeField, Header("キャラクターのデータリスト")]
    private List<CharacterResource> _dataList = new();

    public List<CharacterResource> DataList => _dataList;

    public void AddData(CharacterResource characterData)
    {
        _dataList.Add(characterData);
    }
}

