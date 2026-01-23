using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// トレーニングイベントのシナリオのCSVデータのリポジトリ
/// </summary>
public class AddressableTrainingEventScenarioDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    public string[,] _csvSplitRepositoryData;

    [Inject]
    public AddressableTrainingEventScenarioDataRepository() { }

    public ScenarioData GetScenarioData(uint eventID)
    {
        ScenarioData targetData = null;
        bool isSearchScenario = true;

        for (int column = 0; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if (isSearchScenario)
            {
                if (uint.TryParse(_csvSplitRepositoryData[column, 0], out uint result) && result == eventID)
                {
                    Debug.Log("IDを見つけました");
                    isSearchScenario = false;
                    targetData = new ScenarioData();

                    NovelPageData novelPageData = new NovelPageData
                    {
                        TalkCharacterName = _csvSplitRepositoryData[column, 1],
                        ScenarioData = _csvSplitRepositoryData[column, 2],
                        CharacterCenter = _csvSplitRepositoryData[column, 3],
                        CharacterLeftBottom = _csvSplitRepositoryData[column, 4],
                        CharacterRightBottom = _csvSplitRepositoryData[column, 5],
                        CharacterLeftTop = _csvSplitRepositoryData[column, 6],
                        CharacterRightTop = _csvSplitRepositoryData[column, 7],
                        BackScreenName = _csvSplitRepositoryData[column, 8]
                    };

                    targetData.EnQueuePageData(novelPageData);
                }
            }
            else 
            {
                if(string.IsNullOrEmpty(_csvSplitRepositoryData[column, 0]))
                {
                    NovelPageData novelPageData = new NovelPageData
                    {
                        TalkCharacterName = _csvSplitRepositoryData[column, 1],
                        ScenarioData = _csvSplitRepositoryData[column, 2],
                        CharacterCenter = _csvSplitRepositoryData[column, 3],
                        CharacterLeftBottom = _csvSplitRepositoryData[column, 4],
                        CharacterRightBottom = _csvSplitRepositoryData[column, 5],
                        CharacterLeftTop = _csvSplitRepositoryData[column, 6],
                        CharacterRightTop = _csvSplitRepositoryData[column, 7],
                        BackScreenName = _csvSplitRepositoryData[column, 8]
                    };

                    targetData.EnQueuePageData(novelPageData);
                }
                else
                {
                    return targetData;
                }
            }
        }

        return targetData;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_TrainingCommonEventScenarioDataCSV);
        _csvSplitRepositoryData = CSVLoader.LoadCsv(_repositoryData);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_TrainingCommonEventScenarioDataCSV);

    }
}
