using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// トレーニングイベントのシナリオのCSVデータのリポジトリ
/// </summary>
public class AddressableTrainingEventScenarioDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository, ICSVDataRepository
{
    public string[,] CSVSplitRepositoryData => CSVLoader.LoadCsv(_repositoryData);

    [Inject]
    public AddressableTrainingEventScenarioDataRepository() { }

    public ScenarioData GetScenarioData(uint eventID)
    {
        ScenarioData targetData = null;
        bool isSeachScenario = true;

        for (int column = 0; column < CSVSplitRepositoryData.GetLength(0); column++)
        {
            if (isSeachScenario)
            {
                if (string.IsNullOrEmpty(CSVSplitRepositoryData[column, 0]))
                {
                    continue;
                }
                else if (uint.Parse(CSVSplitRepositoryData[column, 0]) == eventID)
                {
                    isSeachScenario = false;
                    targetData = new ScenarioData();

                    NovelPageData novelPageData = new NovelPageData
                    {
                        TalkCharacterName = CSVSplitRepositoryData[column, 1],
                        ScenarioData = CSVSplitRepositoryData[column, 2],
                        CharacterCenter = CSVSplitRepositoryData[column, 3],
                        CharacterLeftBottom = CSVSplitRepositoryData[column, 4],
                        CharacterRightBottom = CSVSplitRepositoryData[column, 5],
                        CharacterLeftTop = CSVSplitRepositoryData[column, 6],
                        CharacterRightTop = CSVSplitRepositoryData[column, 7],
                        BackScreenName = CSVSplitRepositoryData[column, 8]
                    };

                    targetData.EnQueuePageData(novelPageData);
                    continue;
                }
            }
            else 
            {
                if(string.IsNullOrEmpty(CSVSplitRepositoryData[column, 0]))
                {
                    NovelPageData novelPageData = new NovelPageData
                    {
                        TalkCharacterName = CSVSplitRepositoryData[column, 1],
                        ScenarioData = CSVSplitRepositoryData[column, 2],
                        CharacterCenter = CSVSplitRepositoryData[column, 3],
                        CharacterLeftBottom = CSVSplitRepositoryData[column, 4],
                        CharacterRightBottom = CSVSplitRepositoryData[column, 5],
                        CharacterLeftTop = CSVSplitRepositoryData[column, 6],
                        CharacterRightTop = CSVSplitRepositoryData[column, 7],
                        BackScreenName = CSVSplitRepositoryData[column, 8]
                    };

                    targetData.EnQueuePageData(novelPageData);
                }
                else
                {
                    return targetData;
                }
            }
        }

        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_TrainingCommonEventScenarioDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_TrainingCommonEventScenarioDataCSV);
    }
}
