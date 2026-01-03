using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

/// <summary> キャラクターのベースデータ </summary>
#region CharacterBaseData
[Serializable]
public class CharacterBaseData
{
    //ステータス
    [SerializeField, Header("ID")]
    protected uint _characterID;
    [SerializeField, Header("名前")]
    protected string _characterName;
    [SerializeField, Header("レア度")]
    protected uint _baseRarity;
    [SerializeField, Header("体力")]
    protected uint _basePhysical;
    [SerializeField, Header("筋力")]
    protected uint _basePower;
    [SerializeField, Header("知力")]
    protected uint _baseIntelligence;
    [SerializeField, Header("素早さ")]
    protected uint _baseSpeed;
    [SerializeField, Header("戦闘スタイル")]
    protected JobType _roleType;
    [SerializeField, Header("コスト")]
    protected uint _cost;
    public uint CharacterID => _characterID;
    public string CharacterName => _characterName;
    public uint BaseRarity => _baseRarity;
    public uint BasePhysical => _basePhysical;
    public uint BasePower => _basePower;
    public uint BaseIntelligence => _baseIntelligence;
    public uint BaseSpeed => _baseSpeed;
    public JobType CharacterRole => _roleType;
    public uint Cost => _cost;

    #region パラメータの合計値の参照プロパティ
    public virtual uint TotalPhysical => _basePhysical;
    public virtual uint TotalPower => _basePower;
    public virtual uint TotalIntelligence => _baseIntelligence;
    public virtual uint TotalSpeed => _baseSpeed;
    #endregion

    /// <summary>
    /// パラメータの初期化関数
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="charaName">名前</param>
    /// <param name="rarity">レアリティ</param>
    /// <param name="physi">体力</param>
    /// <param name="pow">筋力</param>
    /// <param name="intelli">知力</param>
    /// <param name="sp">素早さ</param>
    public void InitData(uint id, string charaName, uint rarity, uint physi, uint pow, uint intelli, uint sp, string role, uint cost)
    {
        _characterID = id;
        _characterName = charaName;
        _baseRarity = rarity;
        _basePhysical = physi;
        _basePower = pow;
        _baseIntelligence = intelli;
        _baseSpeed = sp;
        SetCharacterRole(role);
        _cost = cost;
    }

    /// <summary>
    /// ベースデータの登録関数
    /// </summary>
    /// <param name="baseData"></param>
    public void SetBaseData(CharacterBaseData baseData)
    {
        _characterID = baseData.CharacterID;
        _characterName = baseData.CharacterName;
        _baseRarity = baseData.BaseRarity;
        _basePhysical = baseData.BasePhysical;
        _basePower = baseData.BasePower;
        _baseIntelligence = baseData.BaseIntelligence;
        _baseSpeed = baseData.BaseSpeed;
        _roleType = baseData.CharacterRole;
        _cost = baseData.Cost;
    }

    protected void SetCharacterRole(string roleType)
    {
        foreach (string role in Enum.GetNames(typeof(JobType)))
        {
            if (role == roleType)
            {
                Enum.TryParse(role, out _roleType);
            }
        }
    }
}
#endregion



