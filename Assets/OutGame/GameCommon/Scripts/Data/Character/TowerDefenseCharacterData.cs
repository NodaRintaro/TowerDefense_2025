using Newtonsoft.Json;
using System;
using UnityEngine;

/// <summary> トレーニング済みのキャラクターデータ </summary>
[Serializable]
public class TowerDefenseCharacterData : CharacterBaseData
{
    [SerializeField, Header("トレーニング後のキャラクターランク")]
    private RankType _rankType;

    [SerializeField, Header("体力増加値"), JsonProperty]
    private uint _addPhysical;
    [SerializeField, Header("筋力増加値"),JsonProperty]
    private uint _addPower;
    [SerializeField, Header("知力増加値"), JsonProperty]
    private uint _addIntelligence;
    [SerializeField, Header("素早さ増加値"), JsonProperty]
    private uint _addSpeed;

    //ToDo:Skill

    #region 増加値の参照用プロパティ
    public uint AddPhysical => _addPhysical;
    public uint AddPower => _addPower;
    public uint AddIntelligence => _addIntelligence;
    public uint AddSpeed => _addSpeed;
    #endregion

    #region パラメータ合計値の参照プロパティ
    public override uint TotalPhysical => _addPhysical + _basePhysical;
    public override uint TotalPower => _addPower + _basePower;
    public override uint TotalIntelligence => _baseIntelligence;
    public override uint TotalSpeed => _addSpeed + _baseSpeed;
    public uint TotalParameter => TotalPhysical + TotalPower + TotalIntelligence + TotalSpeed;
    #endregion

    public void SetCharacterTrainedParameterData(uint setPhysi, uint setPow, uint setInt, uint setSp)
    {
        _addPhysical = setPhysi;
        _addPower = setPow;
        _addIntelligence = setInt;
        _addSpeed = setSp;
    }

    public void SetCharacterRank(RankType rankType)
    {
        _rankType = rankType;
    }
}