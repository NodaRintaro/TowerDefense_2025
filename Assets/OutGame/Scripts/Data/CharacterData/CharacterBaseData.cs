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
    [SerializeField, Header("コスト")]
    protected uint _cost;
    public uint CharacterID => _characterID;
    public string CharacterName => _characterName;
    public uint BasePhysical => _basePhysical;
    public uint BasePower => _basePower;
    public uint BaseIntelligence => _baseIntelligence;
    public uint BaseSpeed => _baseSpeed;
    public RoleType RoleType => _roleType;
    public uint Cost => _cost;

    /// <summary>
    /// パラメータのセッター
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="charaName">名前</param>
    /// <param name="physi">体力</param>
    /// <param name="pow">筋力</param>
    /// <param name="intelli">知力</param>
    /// <param name="sp">素早さ</param>
    public void InitData(uint id, string charaName, uint physi, uint pow, uint intelli, uint sp, string role, uint cost)
    {
        _characterID = id;
        _characterName = charaName;
        _basePhysical = physi;
        _basePower = pow;
        _baseIntelligence = intelli;
        _baseSpeed = sp;
        SetCharacterRole(role);
        _cost = cost;
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
}
