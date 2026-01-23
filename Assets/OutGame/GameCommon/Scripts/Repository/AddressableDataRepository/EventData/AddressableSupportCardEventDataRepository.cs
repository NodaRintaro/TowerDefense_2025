using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// サポートカード固有イベントのCSVデータのリポジトリ
/// </summary>
public class AddressableSupportCardEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    private string[,] _csvSplitRepositoryData = null;

    [Inject]
    public AddressableSupportCardEventDataRepository() 
    { 
        
    }

    /// <summary> トレーニングイベントの取得 </summary>
    public TrainingEventData GetCsvEventData(uint eventID)
    {
        int targetArrayLength = _csvSplitRepositoryData.GetLength(1);
        string[] targetArray = new string[targetArrayLength];
        for (int column = 0; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if (string.IsNullOrEmpty(_csvSplitRepositoryData[column, 1]))
            {
                continue;
            }
            else if (uint.Parse(_csvSplitRepositoryData[column, 1]) == eventID)
            {
                for (int row = 1; targetArray.Length > row; row++)
                {
                    targetArray[row] = _csvSplitRepositoryData[column, row];
                }
                return TrainingEventDataGenerator.GenerateEventData(targetArray);
            }
        }

        Debug.Log("データが見つかりませんでした");
        return null;
    }

    public List<TrainingEventData> GetSupportCardTrainingEventDataList(uint cardID)
    {
        List<TrainingEventData> supportCardEventList = new List<TrainingEventData>();
        bool isSearchCardID = true;
        int targetArrayLength = _csvSplitRepositoryData.GetLength(1);
        string[] targetArray = new string[targetArrayLength];
        for (int column = 0; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if(!isSearchCardID)
            {
                if (string.IsNullOrEmpty(_csvSplitRepositoryData[column, 0]))
                {
                    for (int row = 1; targetArray.Length > row; row++)
                    {
                        targetArray[row] = _csvSplitRepositoryData[column, row];
                    }
                    supportCardEventList.Add(TrainingEventDataGenerator.GenerateEventData(targetArray));
                }
                else
                {
                    return supportCardEventList;
                }
            }
            else
            {
                uint parsedCardID;
                if (uint.TryParse(_csvSplitRepositoryData[column, 0], out parsedCardID) && cardID == parsedCardID)
                {
                    isSearchCardID = false;
                    continue;
                }
                else
                {
                    continue;
                }
            }
        }

        Debug.Log("データが見つかりませんでした");
        return null;
    }

    /// <summary> トレーニング分岐イベントの取得 </summary>
    public List<TrainingEventData> GetBranchEventData(uint eventID)
    {
        string[] targetArray = new string[_csvSplitRepositoryData.GetLength(1)];
        for (int column = 0; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if (uint.Parse(_csvSplitRepositoryData[column, 1]) == eventID)
            {
                List<TrainingEventData> targetList = new List<TrainingEventData>();
                int branchEventCount = 1;
                while (string.IsNullOrEmpty(_csvSplitRepositoryData[column + branchEventCount, 1]))
                {
                    for (int row = 1; targetArray.Length > row; row++)
                    {
                        targetArray[row] = _csvSplitRepositoryData[column, row];
                    }
                    TrainingEventData targetData = TrainingEventDataGenerator.GenerateEventData(targetArray);
                }
                return targetList;
            }
        }

        Debug.Log("データが見つかりませんでした");
        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_SupportCardEventDataCSV);
        _csvSplitRepositoryData = CSVLoader.LoadCsv(_repositoryData);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_SupportCardEventDataCSV);
    }
}

/// <summary>
/// イベント発生条件
/// </summary>
public struct EventFireParamConditions
{
    public RankType PowerRank;
    public RankType IntelligenceRank;
    public RankType PhysicalRank;
    public RankType SpeedRank;
}

