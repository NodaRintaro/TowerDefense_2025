using System;
using UnityEngine;

/// <summary> トレーニング済みのキャラクターデータ </summary>
#region TrainedCharacterData
[Serializable]
public class TrainedCharacterData : CharacterBaseData
{
    [SerializeField, Header("トレーニング後のキャラクターID")]
    private int _trainiedID;

    [SerializeField, Header("トレーニング後のキャラクターランク")]
    private RankType _rankType;

    [SerializeField, Header("体力増加値")]
    private uint _addPhysical;
    [SerializeField, Header("筋力増加値")]
    private uint _addPower;
    [SerializeField, Header("知力増加値")]
    private uint _addIntelligence;
    [SerializeField, Header("素早さ増加値")]
    private uint _addSpeed;

    /// <summary> トレーニング後のキャラクターデータID </summary>
    public int TrainedCharacterID => _trainiedID;

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
    #endregion

    public void SetCharacterTrainedParameterData(int newID, uint setPhysi, uint setPow, uint setInt, uint setSp)
    {
        _trainiedID = newID;
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
#endregion