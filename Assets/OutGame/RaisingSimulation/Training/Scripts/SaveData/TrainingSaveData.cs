using Newtonsoft.Json;
using RaisingSimulationGameFlowStateMachine;
using UnityEngine;

/// <summary>
/// トレーニングで使用するデータの保存クラス
/// </summary>
public class TrainingSaveData : IJsonSaveData
{
    //トレーニング中のキャラクター
    [JsonProperty] private TrainingCharacterData _characterData = null;

    //トレーニングで使用するサポートカード
    [JsonProperty] private TrainingSupportCardDeckData _cardDeckData = new();

    //トレーニング開始からの日数
    [JsonProperty] private uint _currentElapsedDays = 0;

    //スタミナの最大値
    [JsonProperty] private uint _maxStamina = 100;

    //現在のスタミナ
    [JsonProperty] private uint _currentStamina = 100;

    //現在のゲームスクリーン
    [JsonProperty] private ScreenType _currentScreenType;

    //現在のキャラクターのトレーニングスケジュール
    [JsonProperty] private CharacterTrainingSchedule _currentCharacterSchedule = null;

    #region 各種参照プロパティ
    public TrainingCharacterData TrainingCharacterData => _characterData;
    public TrainingSupportCardDeckData TrainingCardDeckData => _cardDeckData;
    public uint CurrentElapsedDays => _currentElapsedDays;
    public uint MaxStamina => _maxStamina;
    public uint CurrentStamina => _currentStamina;
    public ScreenType CurrentScreenType => _currentScreenType;
    public CharacterTrainingSchedule CurrentCharacterSchedule => _currentCharacterSchedule;
    #endregion

    /// <summary> トレーニング対象のキャラクターデータをセット </summary>
    public void SetCharacterBaseData(CharacterBaseData characterData)
    {
        TrainingCharacterData trainingCharacterData = new TrainingCharacterData();
        trainingCharacterData.SetBaseData(characterData);
        _characterData = trainingCharacterData;
    }

    /// <summary> 開始からの日数を経過させる処理 </summary>
    public void AddElapsedDays() => _currentElapsedDays++;

    /// <summary> トレーニング対象のサポートカードデータをセット </summary>
    public void SetCardData(uint deckNum, SupportCardData cardData) => _cardDeckData.CardPutInDeck(deckNum, cardData);

    /// <summary> スタミナを使う処理 </summary>
    public void UseStamina(uint stamina) => _currentStamina -= stamina;

    /// <summary> スタミナの最大値をセットする </summary>
    public void SetMaxStamina(uint stamina) => _maxStamina = stamina;

    /// <summary> スタミナを回復する処理 </summary>
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

    /// <summary> キャラクターの育成スケジュールを決定する </summary>
    public void SetCharacterSchedule(CharacterTrainingSchedule characterTrainingSchedule)
    {
        _currentCharacterSchedule = characterTrainingSchedule;
    }
}