using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Training�Ɋւ���f�[�^��ێ�����Class
/// </summary>
public class TrainingDataRegistry : ScriptableObject
{
    
}

[System.Serializable]
public class TrainingCharacterData
{
    [SerializeField, Header("�L�����N�^�[�̃x�[�X�f�[�^")]
    private CharacterData _baseCharacterData;

    [SerializeField, Header("�̗͂̑����l")]
    private uint _currentPhysicalBuff = 0;

    [SerializeField, Header("�U���͂̑����l")]
    private uint _currentPowerBuff = 0;

    [SerializeField, Header("�m�͂̑����l")]
    private uint _currentIntelligenceBuff = 0;

    [SerializeField, Header("�f�����̑����l")]
    private uint _currentSpeedBuff = 0;

    [SerializeField, Header("�X�^�~�i�̍ő�l")]
    private uint _maxStamina;

    [SerializeField, Header("�L�����N�^�[�̃X�^�~�i")]
    private uint _currentStamina;

    public CharacterData BaseCharacterData => _baseCharacterData;

    #region �e��p�����[�^�̎Q�Ɨp�v���p�e�B
    public uint CurrentPhysicalBuff => _currentPhysicalBuff;
    public uint CurrentPowerBuff => _currentPowerBuff;
    public uint CurrentIntelligenceBuff => _currentIntelligenceBuff;
    public uint CurrentSpeedBuff => _currentSpeedBuff;
    public uint CurrentStamina => _currentStamina;
    #endregion

    public void SetBaseCharacter(CharacterData baseCharacter) => _baseCharacterData = baseCharacter;

    #region �e��p�����[�^�̑�������
    public void AddCurrentPhysical(uint physical) => _currentPhysicalBuff += physical;
    public void AddCurrentPower(uint power) => _currentPowerBuff += power;
    public void AddCurrentIntelligence(uint intelligence) => _currentIntelligenceBuff += intelligence;
    public void AddCurrentSpeed(uint speed) => _currentSpeedBuff += speed;
    public void UseStamina(uint stamina) => _currentStamina -= stamina;
    public void TakeBreak(uint stamina) => _currentStamina += stamina;
    #endregion
}