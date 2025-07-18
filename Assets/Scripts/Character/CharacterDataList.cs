using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName ="CharacterDataList",menuName = "ScriptableObject/CharacterDataList")]
public class CharacterDataList : ScriptableObject
{
    [SerializeField,Header("キャラクターのデータリスト")]
    private List<CharacterData> _dataList = new();
    
    public List<CharacterData> DataList => _dataList;

    public void AddData(CharacterData characterData)
    {
        _dataList.Add(characterData);
    }
}

[System.Serializable]
public class CharacterData
{
    [SerializeField, Header("ID")]
    private uint _id;
    [SerializeField, Header("名前")]
    private string _characterName;
    [SerializeField, Header("体力")]
    private uint _physical;
    [SerializeField, Header("筋力")]
    private uint _power;
    [SerializeField, Header("知力")]
    private uint _intelligence;
    [SerializeField, Header("素早さ")]
    private uint _speed;

    /// <summary>
    /// パラメータのセッター
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="characterName">名前</param>
    /// <param name="physical">体力</param>
    /// <param name="power">筋力</param>
    /// <param name="intelligence">知力</param>
    /// <param name="speed">素早さ</param>
    public void InitData(string id, string characterName, string physical, string power, string intelligence, string speed)
    {
        _id = uint.Parse(id);
        _characterName = characterName;
        _physical = uint.Parse(physical);
        _power = uint.Parse(power);
        _intelligence = uint.Parse(intelligence);
        _speed = uint.Parse(speed);
    }
    
    public uint ID => _id;
    
    public string CharacterName => _characterName;
    
    public uint Physical => _physical;
    
    public uint Power => _power;
    
    public uint Intelligence => _intelligence;
    
    public uint Speed => _speed;
}