using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField,Header("キャラクターのベースデータの保存クラス")]
    private CharacterDataList _characterDataList = default;

    [SerializeField, Header("トレーニングするキャラクターのベースデータ")]
    private static TrainingCharacterData _currentTrainigCharacter = default;

    [SerializeField, Header("レイドイベント開始までのカウントダウン")]
    private uint _raidEventCountDown;

    [SerializeField, Header("Trainingの種類一覧")]
    private List<TrainingMenu> _trainingMenuList = new();

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
    [SerializeField,Header("キャラクターのベースデータ")]
    private CharacterData _baseCharacterData;

    [SerializeField,Header("体力の増加値")]
    private uint _currentPhysical;

    [SerializeField, Header("攻撃力の増加値")]
    private uint _currentPower;

    [SerializeField, Header("知力の増加値")]
    private uint _currentIntelligence;

    [SerializeField, Header("素早さの増加値")]
    private uint _currentSpeed;

    [SerializeField, Header("キャラクターのスタミナ")]
    private uint _currentStamina;

    public CharacterData BaseCharacterData => _baseCharacterData;

    #region 各種パラメータの参照用プロパティ
    public uint CurrentPhysical => _currentPhysical;
    public uint CurrentPower => _currentPower;
    public uint CurrentIntelligence => _currentIntelligence;
    public uint CurrentSpeed => _currentSpeed;
    public uint CurrentStamina => _currentStamina;
    #endregion

    public void SetBaseCharacter(CharacterData baseCharacter) => _baseCharacterData = baseCharacter;

    #region 各種パラメータの増加処理
    public void AddCurrentPhysical(uint physical) => _currentPhysical += physical;
    public void AddCurrentPower(uint power) => _currentPower += power;
    public void AddCurrentIntelligence(uint intelligence) => _currentIntelligence += intelligence;
    public void AddCurrentSpeed(uint speed) => _currentSpeed += speed;
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
