using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> キャラクターのベースデータ </summary>
[System.Serializable]
public class CharacterBaseData
{
    //ステータス
    [SerializeField, Header("ID")]
    protected uint _characterID;
    [SerializeField, Header("名前")]
    protected string _characterName;
    [SerializeField, Header("体力")]
    protected uint _basePhysical;
    [SerializeField, Header("筋力")]
    protected uint _basePower;
    [SerializeField, Header("知力")]
    protected uint _baseIntelligence;
    [SerializeField, Header("素早さ")]
    protected uint _baseSpeed;
    [SerializeField, Header("戦闘スタイル")]
    protected RoleType _roleType;

    //所持済みの判定
    [SerializeField, Header("所持済みかどうかの判定")]
    protected bool _isGetting = false;

    public uint CharacterID => _characterID;
    public string CharacterName => _characterName;
    public uint BasePhysical => _basePhysical;
    public uint BasePower => _basePower;
    public uint BaseIntelligence => _baseIntelligence;
    public uint BaseSpeed => _baseSpeed;
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
        _characterID = uint.Parse(id);
        _characterName = charaName;
        _basePhysical = uint.Parse(physi);
        _basePower = uint.Parse(pow);
        _baseIntelligence = uint.Parse(intelli);
        _baseSpeed = uint.Parse(sp);
        SetCharacterRole(role);
    }

    private void SetCharacterRole(string roleType)
    {
        switch (roleType)
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
