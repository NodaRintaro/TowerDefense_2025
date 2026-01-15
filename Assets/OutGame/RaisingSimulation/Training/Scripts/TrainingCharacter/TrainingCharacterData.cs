using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

/// <summary> トレーニング中のキャラクターデータ </summary>
[System.Serializable]
public class TrainingCharacterData : CharacterBaseData
{
    [Inject]
    public TrainingCharacterData(CharacterBaseData data)
    {
        SetBaseData(data);
    }

    //各種パラメータの増加値
    [SerializeField, Header("体力の増加値")]
    private uint _currentPhysicalBuff = 0;
    [SerializeField, Header("攻撃力の増加値")]
    private uint _currentPowerBuff = 0;
    [SerializeField, Header("知力の増加値")]
    private uint _currentIntelligenceBuff = 0;
    [SerializeField, Header("素早さの増加値")]
    private uint _currentSpeedBuff = 0;

    #region 各種パラメータのベースパラメータと強化値の合計値
    public override uint TotalPhysical => _currentPhysicalBuff + base.BasePhysical;
    public override uint TotalPower => _currentPowerBuff + BasePower;
    public override uint TotalIntelligence => _currentIntelligenceBuff + BaseIntelligence;
    public override uint TotalSpeed => _currentSpeedBuff + BaseSpeed;
    #endregion

    #region 各種パラメータの参照用プロパティ
    public uint CurrentPhysicalBuff => _currentPhysicalBuff;
    public uint CurrentPowerBuff => _currentPowerBuff;
    public uint CurrentIntelligenceBuff => _currentIntelligenceBuff;
    public uint CurrentSpeedBuff => _currentSpeedBuff;
    #endregion

    #region 各種パラメータの増加処理
    public void AddCurrentPhysical(uint physical) => _currentPhysicalBuff += physical;
    public void AddCurrentPower(uint power) => _currentPowerBuff += power;
    public void AddCurrentIntelligence(uint intelligence) => _currentIntelligenceBuff += intelligence;
    public void AddCurrentSpeed(uint speed) => _currentSpeedBuff += speed;
    #endregion
}