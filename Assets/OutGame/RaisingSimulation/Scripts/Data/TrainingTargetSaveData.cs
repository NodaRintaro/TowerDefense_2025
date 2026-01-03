using VContainer;

/// <summary>
/// トレーニングで使用するデータの保存クラス
/// </summary>
public class TrainingTargetSaveData : IJsonSaveData
{
    private TrainingCharacterData _characterData = null;

    private TrainingSupportCardDeckData _cardDeckData = new();

    public TrainingCharacterData TrainingCharacterData => _characterData;

    public TrainingSupportCardDeckData TrainingCardDeckData => _cardDeckData;

    [Inject]
    public TrainingTargetSaveData() { }

    /// <summary> トレーニング対象のキャラクターデータをセット </summary>
    public void SetCharacterData(CharacterBaseData characterData)
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
    public void SetCardData(uint deckNum, SupportCardData cardData)
    {
        _cardDeckData.CardPutInDeck(deckNum, cardData);
    }
}