using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// サポートカード固有イベントのシナリオのCSVデータのリポジトリ
/// </summary>
public class AddressableSupportCardEventScenarioDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository , ICSVDataRepository
{
    public string[,] CSVSplitRepositoryData => CSVLoader.LoadCsv(_repositoryData);

    [Inject]
    public AddressableSupportCardEventScenarioDataRepository() { }

    public ScenarioData GetScenarioData(uint eventID)
    {
        ScenarioData targetData = null;
        bool isSeachScenario = true;

        for (int column = 1; column < CSVSplitRepositoryData.GetLength(0); column++)
        {
            if (isSeachScenario)
            {
                if (string.IsNullOrEmpty(CSVSplitRepositoryData[column, 1]))
                {
                    continue;
                }
                else if (uint.Parse(CSVSplitRepositoryData[column, 1]) == eventID)
                {
                    isSeachScenario = false;
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

        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_SupportCardScenarioDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_SupportCardScenarioDataCSV);
    }
}