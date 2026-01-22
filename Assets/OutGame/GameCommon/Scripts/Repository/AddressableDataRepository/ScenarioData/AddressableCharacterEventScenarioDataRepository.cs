using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// キャラクター固有イベントのシナリオのCSVデータのリポジトリ
/// </summary>
public class AddressableCharacterEventScenarioDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository, ICSVDataRepository
{
    public string[,] CSVSplitRepositoryData => CSVLoader.LoadCsv(_repositoryData);

    [Inject]
    public AddressableCharacterEventScenarioDataRepository() { }

    public ScenarioData GetScenarioData(uint characterID, uint eventID)
    {
        ScenarioData targetData = null;
        bool isSearchCharacter = true;
        bool isSearchScenario = true;

        for (int column = 0; column < CSVSplitRepositoryData.GetLength(0); column++)
        {
            if (isSearchCharacter)
            {
                if (string.IsNullOrEmpty(CSVSplitRepositoryData[column, 0])) continue;
                else if (uint.Parse(CSVSplitRepositoryData[column, 0]) == characterID)
                {
                    isSearchCharacter = false;
                    continue;
                }
            }
            else
            {
                if(isSearchScenario)
                {
                    if (string.IsNullOrEmpty(CSVSplitRepositoryData[column, 1])) continue;
                    else if (uint.Parse(CSVSplitRepositoryData[column, 1]) == eventID)
                    {
                        isSearchScenario = false;
                        targetData = new ScenarioData();

                        NovelPageData novelPageData = new NovelPageData
                        {
                            TalkCharacterName = CSVSplitRepositoryData[column, 2],
                            ScenarioData = CSVSplitRepositoryData[column, 3],
                            CharacterCenter = CSVSplitRepositoryData[column, 4],
                            CharacterLeftBottom = CSVSplitRepositoryData[column, 5],
                            CharacterRightBottom = CSVSplitRepositoryData[column, 6],
                            CharacterLeftTop = CSVSplitRepositoryData[column, 7],
                            CharacterRightTop = CSVSplitRepositoryData[column, 8],
                            BackScreenName = CSVSplitRepositoryData[column, 9]
                        };

                        targetData.EnQueuePageData(novelPageData);
                        continue;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(CSVSplitRepositoryData[column, 1]))
                    {
                        NovelPageData novelPageData = new NovelPageData
                        {
                            TalkCharacterName = CSVSplitRepositoryData[column, 2],
                            ScenarioData = CSVSplitRepositoryData[column, 3],
                            CharacterCenter = CSVSplitRepositoryData[column, 4],
                            CharacterLeftBottom = CSVSplitRepositoryData[column, 5],
                            CharacterRightBottom = CSVSplitRepositoryData[column, 6],
                            CharacterLeftTop = CSVSplitRepositoryData[column, 7],
                            CharacterRightTop = CSVSplitRepositoryData[column, 8],
                            BackScreenName = CSVSplitRepositoryData[column, 9]
                        };

                        targetData.EnQueuePageData(novelPageData);
                    }
                    else
                    {
                        return targetData;
                    }
                }
            }
        }

        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_CharacterEventScenarioDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_CharacterEventScenarioDataCSV);
    }
}
