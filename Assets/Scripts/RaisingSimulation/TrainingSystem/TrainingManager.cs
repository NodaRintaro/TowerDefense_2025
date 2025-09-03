using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField, Header("キャラクターのデータのデータの保存先")]
    private string _characterDataPath = "CharacterData";

    [SerializeField, Header("トレーニングするキャラクターのベースデータ")]
    private TrainingCharacterData _currentTrainigCharacter = default;

    [SerializeField, Header("レイドイベント開始までのカウントダウン")]
    private uint _raidEventCountDown;

    [SerializeField, Header("Trainingの種類一覧")]
    private TrainingMenu[] _trainingMenuList = default;

    private int _trainingCharacterID = 0;

    private CharacterDataList _characterDataList = default;

    public TrainingCharacterData CurrentTrainingCharacterData => _currentTrainigCharacter;

    public TrainingMenu[] TrainingMenuList => _trainingMenuList;

    private void Start()
    {
        _characterDataList = Resources.Load<CharacterDataList>(_characterDataPath);
    }

    public void SetCharacterID(int id) => _trainingCharacterID = id;

    public void RaidCountDown() => _raidEventCountDown--;

    /// <summary> トレーニングの開始時の処理 </summary>
    /// <param name="characterID"> キャラのID </param>
    public void StartTrainig(int characterID)
    {
        _currentTrainigCharacter = new();

        _currentTrainigCharacter.SetBaseCharacter(CharacterDataFind(characterID));

        
    }

    /// <summary> キャラクターのトレーニングが終了したときの処理 </summary>
    public void FinishTraining()
    {

        _currentTrainigCharacter = null;
    }

    /// <summary> IDからCharacterDataを探す処理 </summary>
    private CharacterData CharacterDataFind(int characterID)
    {
        CharacterData characterData = null;
        foreach (var character in _characterDataList.DataList)
        {
            if (character.ID == characterID)
            {
                characterData = character;
            }
        }
        return characterData;
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

public enum TrainingType
{
    Physical,
    Power,
    Intelligence,
    Speed,
    TakeBreak
}
