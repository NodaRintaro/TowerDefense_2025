using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


/// <summary>
/// TrainingCharcterに関するデータを保持するClass
/// </summary>
public class TrainingDataHolder
{
    private TrainingCharacterData _trainingCharacterData = null;

    private SupportCardData[] _selectedSupportCardData = null;

    public TrainingCharacterData TrainingCharacterData => _trainingCharacterData;
    public SupportCardData[] SelectedSupportCards => _selectedSupportCardData;

    [Inject]
    public TrainingDataHolder()
    {
        Init();
    }

    public void Init()
    {
        ClearData();
    }

    public void SetCharacterData(CharacterData characterData)
    {
        _trainingCharacterData = new TrainingCharacterData();
        _trainingCharacterData.SetBaseCharacter(characterData);
        _trainingCharacterData.TakeBreak(_trainingCharacterData.MaxStamina);
    }

    public void SetSupportCardsData(SupportCardData[] supportCardDatas)
    {
        _selectedSupportCardData = supportCardDatas;
    }

    public void FinishTrainingData()
    {
        TrainedCharacterData trainedCharacterData = new TrainedCharacterData();
        string newID = Guid.NewGuid().ToString();
        
        trainedCharacterData.SetCharacterTrainedData(
            _trainingCharacterData.BaseCharacterData,
            newID,
            _trainingCharacterData.CurrentPhysicalBuff,
            _trainingCharacterData.CurrentPowerBuff,
            _trainingCharacterData.CurrentIntelligenceBuff,
            _trainingCharacterData.CurrentSpeedBuff
            );

        ClearData();
    }

    public void ClearData() 
    { 
        _trainingCharacterData = null; 
        _selectedSupportCardData = null;
    }
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

    public void SetBaseCharacter(CharacterData baseCharacter) => _baseCharacterData = baseCharacter;

    public void SetMaxStamina(uint stamina) => _maxStamina = stamina;

    #region 各種パラメータのベースパラメータと強化値の合計値
    public uint TotalPhysical => _currentPhysicalBuff + _baseCharacterData.Physical;
    public uint TotalPower => _currentPowerBuff + _baseCharacterData.Power;
    public uint TotalIntelligence => _currentIntelligenceBuff + _baseCharacterData.Intelligence;
    public uint TotalSpeed => _currentSpeedBuff + _baseCharacterData.Speed;
    #endregion

    #region 各種パラメータの参照用プロパティ
    public uint CurrentPhysicalBuff => _currentPhysicalBuff;
    public uint CurrentPowerBuff => _currentPowerBuff;
    public uint CurrentIntelligenceBuff => _currentIntelligenceBuff;
    public uint CurrentSpeedBuff => _currentSpeedBuff;
    public uint MaxStamina => _maxStamina;
    public uint CurrentStamina => _currentStamina;
    #endregion

    #region 各種パラメータの増加処理
    public void AddCurrentPhysical(uint physical) => _currentPhysicalBuff += physical;
    public void AddCurrentPower(uint power) => _currentPowerBuff += power;
    public void AddCurrentIntelligence(uint intelligence) => _currentIntelligenceBuff += intelligence;
    public void AddCurrentSpeed(uint speed) => _currentSpeedBuff += speed;
    public void UseStamina(uint stamina) => _currentStamina -= stamina;
    public void TakeBreak(uint stamina)
    {
        if(_currentStamina + stamina > _maxStamina)
        {
            _currentStamina = _maxStamina;
        }
        else
        {
            _currentStamina += stamina;
        }
    }
    #endregion
}