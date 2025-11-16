using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

[System.Serializable]
public class TrainingCharacterData : CharacterBaseData
{
    [Inject]
    public TrainingCharacterData(CharacterBaseData data)
    {
        InitData(
            data.CharacterID, 
            data.CharacterName, 
            data.BasePhysical, 
            data.BasePower, 
            data.BaseIntelligence,
            data.BaseSpeed, 
            data.RoleType.ToString());
    }

    //各種パラメータの増加値
    [SerializeField, Header("体力の増加値")]
    private ReactiveProperty<uint> _currentPhysicalBuff = new ReactiveProperty<uint>(0);
    [SerializeField, Header("攻撃力の増加値")]
    private ReactiveProperty<uint> _currentPowerBuff = new ReactiveProperty<uint>(0);
    [SerializeField, Header("知力の増加値")]
    private ReactiveProperty<uint> _currentIntelligenceBuff = new ReactiveProperty<uint>(0);
    [SerializeField, Header("素早さの増加値")]
    private ReactiveProperty<uint> _currentSpeedBuff = new ReactiveProperty<uint>(0);

    private Dictionary<ParameterType, RankData> _parameterRankDict = new();

    [SerializeField, Header("スタミナの最大値")]
    private uint _maxStamina;

    [SerializeField, Header("キャラクターのスタミナ")]
    private uint _currentStamina;

    public Dictionary<ParameterType, RankData> ParameterRankDict => _parameterRankDict;

    #region 各種パラメータのベースパラメータと強化値の合計値
    public uint TotalPhysical => _currentPhysicalBuff.Value + BasePhysical;
    public uint TotalPower => _currentPowerBuff.Value + BasePower;
    public uint TotalIntelligence => _currentIntelligenceBuff.Value + BaseIntelligence;
    public uint TotalSpeed => _currentSpeedBuff.Value + BaseSpeed;
    #endregion

    #region 各種パラメータの参照用プロパティ
    public ReactiveProperty<uint> CurrentPhysicalBuff => _currentPhysicalBuff;
    public ReactiveProperty<uint> CurrentPowerBuff => _currentPowerBuff;
    public ReactiveProperty<uint> CurrentIntelligenceBuff => _currentIntelligenceBuff;
    public ReactiveProperty<uint> CurrentSpeedBuff => _currentSpeedBuff;
    public uint MaxStamina => _maxStamina;
    public uint CurrentStamina => _currentStamina;
    #endregion

    #region 各種パラメータの増加処理
    public void AddCurrentPhysical(uint physical) => _currentPhysicalBuff.Value += physical;
    public void AddCurrentPower(uint power) => _currentPowerBuff.Value += power;
    public void AddCurrentIntelligence(uint intelligence) => _currentIntelligenceBuff.Value += intelligence;
    public void AddCurrentSpeed(uint speed) => _currentSpeedBuff.Value += speed;
    public void UseStamina(uint stamina) => _currentStamina -= stamina;
    public void TakeBreak(uint stamina)
    {
        if (_currentStamina + stamina > _maxStamina)
        {
            _currentStamina = _maxStamina;
        }
        else
        {
            _currentStamina += stamina;
        }
    }
    #endregion

    public void SetMaxStamina(uint stamina) => _maxStamina = stamina;
}