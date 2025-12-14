using VContainer;
using CharacterData;

/// <summary>
/// トレーニングで使用するデータを構築し保存するクラス
/// </summary>
public class TrainingDataBuilder
{
    private TrainingSaveData _trainingData = null;

    private readonly SupportCardData[] _selectedSupportCardData = null;

    private const int _supportCardDeckNum = 4;

    private const string _trainingSaveDataName = "TrainingSaveData";

    [Inject]
    public TrainingDataBuilder()
    {
        _selectedSupportCardData = new SupportCardData[_supportCardDeckNum];
    }

    /// <summary> トレーニングするキャラクターの選択 </summary>
    public void SetTrainingCharacter(CharacterBaseData characterBaseData)
    {
        _trainingData.SetCharacterData(characterBaseData);
    }

    /// <summary> トレーニングで使用するサポートカードをセット </summary>
    public void SetSupportCard(int cardDeckNum, SupportCardData supportCardData)
    {
        _selectedSupportCardData[cardDeckNum] = supportCardData;
    }

    public void TrainingDataSave()
    {
        //DataSaveSystem.DataSave(_trainingData, _trainingSaveDataName);
    }
}
