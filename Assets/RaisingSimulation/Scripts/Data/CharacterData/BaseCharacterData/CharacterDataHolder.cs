using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterDataList",menuName = "ScriptableObject/CharacterDataList")]
public class CharacterDataHolder : ScriptableObject
{
    [SerializeField,Header("キャラクターのデータリスト")]
    private List<CharacterData> _dataList = new();
    
    public List<CharacterData> DataList => _dataList;

    public void AddData(CharacterData characterData)
    {
        _dataList.Add(characterData);
    }
}

/// <summary> キャラクターのベースデータ </summary>
[System.Serializable]
public class CharacterData
{
    //ステータス
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
    [SerializeField, Header("戦闘スタイル")]
    private RoleType _roleType;

    //所持済みの判定
    [SerializeField, Header("所持済みかどうかの判定")]
    private bool _isGetting = false;

    public uint ID => _id;
    public string CharacterName => _characterName;
    public uint Physical => _physical;
    public uint Power => _power;
    public uint Intelligence => _intelligence;
    public uint Speed => _speed;
    public RoleType RoleType => _roleType;
    public bool IsGetting => _isGetting;

    /// <summary>
    /// パラメータのセッター
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="charaName">名前</param>
    /// <param name="physi">体力</param>
    /// <param name="pow">筋力</param>
    /// <param name="intelli">知力</param>
    /// <param name="sp">素早さ</param>
    public void InitData(string id, string charaName, string physi, string pow, string intelli, string sp, string role)
    {
        _id = uint.Parse(id);
        _characterName = charaName;
        _physical = uint.Parse(physi);
        _power = uint.Parse(pow);
        _intelligence = uint.Parse(intelli);
        _speed = uint.Parse(sp);
        SetCharacterRole(role);
    }

    private void SetCharacterRole(string roleType)
    {
        switch(roleType)
        {
            case "Attacker":
                _roleType = RoleType.Attacker;
                break;
            case "Tank":
                _roleType = RoleType.Tank;
                break;
            case "Magic":
                _roleType = RoleType.Magic;
                break;
            case "Sniper":
                _roleType = RoleType.Sniper;
                break;
            case "Healer":
                _roleType = RoleType.Healer;
                break;
            case "Supporter":
                _roleType = RoleType.Supporter;
                break;
            case "Special":
                _roleType = RoleType.Special;
                break;
            default:
                _roleType = RoleType.Null;
                break;
        }
    }

    public void GetCharacter() => _isGetting = true;
}

public enum RoleType
{
    Null,
    Attacker,
    Tank,
    Magic,
    Sniper,
    Healer,
    Supporter,
    Special
}