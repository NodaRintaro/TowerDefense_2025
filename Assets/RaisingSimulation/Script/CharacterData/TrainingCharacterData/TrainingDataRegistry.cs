using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trainingに関するデータを保持するClass
/// </summary>
public class TrainingDataRegistry : ScriptableObject
{
    
}

[System.Serializable]
public class TrainingCharacterData
{
    [SerializeField, Header("キャラクターのベースデータ")]
    private CharacterData _baseCharacterData;

    [SerializeField, Header("体力の増加値")]
    private uint _currentPhysicalBuff = 0;

    [SerializeField, Header("攻撃力の増加値")]
    private uint _currentPowerBuff = 0;

    [SerializeField, Header("知力の増加値")]
    private uint _currentIntelligenceBuff = 0;

    [SerializeField, Header("素早さの増加値")]
    private uint _currentSpeedBuff = 0;

    [SerializeField, Header("スタミナの最大値")]
    private uint _maxStamina;

    [SerializeField, Header("キャラクターのスタミナ")]
    private uint _currentStamina;

    public CharacterData BaseCharacterData => _baseCharacterData;

    #region 各種パラメータの参照用プロパティ
    public uint CurrentPhysicalBuff => _currentPhysicalBuff;
    public uint CurrentPowerBuff => _currentPowerBuff;
    public uint CurrentIntelligenceBuff => _currentIntelligenceBuff;
    public uint CurrentSpeedBuff => _currentSpeedBuff;
    public uint CurrentStamina => _currentStamina;
    #endregion

    public void SetBaseCharacter(CharacterData baseCharacter) => _baseCharacterData = baseCharacter;

    #region 各種パラメータの増加処理
    public void AddCurrentPhysical(uint physical) => _currentPhysicalBuff += physical;
    public void AddCurrentPower(uint power) => _currentPowerBuff += power;
    public void AddCurrentIntelligence(uint intelligence) => _currentIntelligenceBuff += intelligence;
    public void AddCurrentSpeed(uint speed) => _currentSpeedBuff += speed;
    public void UseStamina(uint stamina) => _currentStamina -= stamina;
    public void TakeBreak(uint stamina) => _currentStamina += stamina;
    #endregion
}