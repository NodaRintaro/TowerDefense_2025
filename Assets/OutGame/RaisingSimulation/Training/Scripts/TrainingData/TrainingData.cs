using RaisingSimulationGameFlowStateMachine;
using UnityEngine;

/// <summary>
/// トレーニングで使用するデータの保存クラス
/// </summary>
public class TrainingData : IJsonSaveData
{
    //トレーニング中のキャラクター
    private TrainingCharacterData _characterData = null;

    //トレーニングで使用するサポートカード
    private TrainingSupportCardDeckData _cardDeckData = new();

    //スタミナの最大値
    private uint _maxStamina = 100;

    //現在のスタミナ
    private uint _currentStamina = 100;

    //現在のゲームスクリーン
    private ScreenType _currentScreenType;

    #region 各種参照プロパティ
    public TrainingCharacterData TrainingCharacterData => _characterData;
    public TrainingSupportCardDeckData TrainingCardDeckData => _cardDeckData;
    public uint MaxStamina => _maxStamina;
    public uint CurrentStamina => _currentStamina;
    public ScreenType CurrentScreenType => _currentScreenType;
    #endregion

    /// <summary> トレーニング対象のキャラクターデータをセット </summary>
    public void SetCharacterBaseData(CharacterBaseData characterData)
    {
        if(_characterData == null)
        {
            _characterData = new TrainingCharacterData(characterData);
        }
        else
        {
            _characterData.SetBaseData(characterData);
        }
    }

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
}