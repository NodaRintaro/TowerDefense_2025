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
public class AddressableSupportCardEventScenarioDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    private string[,] _csvSplitRepositoryData;

    [Inject]
    public AddressableSupportCardEventScenarioDataRepository() { }

    public ScenarioData GetScenarioData(uint eventID)
    {
        ScenarioData targetData = null;
        bool isSeachScenario = true;

        for (int column = 1; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if (isSeachScenario)
            {
                if (string.IsNullOrEmpty(_csvSplitRepositoryData[column, 1]))
                {
                    continue;
                }
                else if (uint.Parse(_csvSplitRepositoryData[column, 1]) == eventID)
                {
                    isSeachScenario = false;
                    targetData = new ScenarioData();

                    NovelPageData novelPageData = new NovelPageData
                    {
                        TalkCharacterName = _csvSplitRepositoryData[column, 2],
                        ScenarioData = _csvSplitRepositoryData[column, 3],
                        CharacterCenter = _csvSplitRepositoryData[column, 4],
                        CharacterLeftBottom = _csvSplitRepositoryData[column, 5],
                        CharacterRightBottom = _csvSplitRepositoryData[column, 6],
                        CharacterLeftTop = _csvSplitRepositoryData[column, 7],
                        CharacterRightTop = _csvSplitRepositoryData[column, 8],
                        BackScreenName = _csvSplitRepositoryData[column, 9]
                    };

                    targetData.EnQueuePageData(novelPageData);
                    continue;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_csvSplitRepositoryData[column, 1]))
                {
                    NovelPageData novelPageData = new NovelPageData
                    {
                        TalkCharacterName = _csvSplitRepositoryData[column, 2],
                        ScenarioData = _csvSplitRepositoryData[column, 3],
                        CharacterCenter = _csvSplitRepositoryData[column, 4],
                        CharacterLeftBottom = _csvSplitRepositoryData[column, 5],
                        CharacterRightBottom = _csvSplitRepositoryData[column, 6],
                        CharacterLeftTop = _csvSplitRepositoryData[column, 7],
                        CharacterRightTop = _csvSplitRepositoryData[column, 8],
                        BackScreenName = _csvSplitRepositoryData[column, 9]
                    };

                    targetData.EnQueuePageData(novelPageData);
                }
                else
                {
                    return targetData;
                }
            }
        }

        Debug.Log("データが見つかりませんでした");
        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_SupportCardScenarioDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_SupportCardScenarioDataCSV);
        _csvSplitRepositoryData = CSVLoader.LoadCsv(_repositoryData);
    }
}