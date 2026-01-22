using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// サポートカード固有イベントのCSVデータのリポジトリ
/// </summary>
public class AddressableSupportCardEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository, ICSVDataRepository
{
    public string[,] CSVSplitRepositoryData => CSVLoader.LoadCsv(_repositoryData);

    [Inject]
    public AddressableSupportCardEventDataRepository() { }

    public string[] GetCsvEventData(uint eventID)
    {
        int targetArrayLength = CSVSplitRepositoryData.GetLength(1);
        string[] targetArray = new string[targetArrayLength];
        for (int column = 0; column < CSVSplitRepositoryData.GetLength(0); column++)
        {
            if (string.IsNullOrEmpty(CSVSplitRepositoryData[column, 1]))
            {
                continue;
            }
            else if (uint.Parse(CSVSplitRepositoryData[column, 1]) == eventID)
            {
                for (int row = 1; targetArray.Length > row; row++)
                {
                    targetArray[row] = CSVSplitRepositoryData[column, row];
                }
                return targetArray;
            }
        }
        return null;
    }

    public List<string[]> GetBranchEvent(uint eventID)
    {
        //List<string[]> strings = new List<string[]>();
        //int targetArrayLength = CSVSplitRepositoryData.GetLength(1);
        //string[] targetArray = new string[targetArrayLength];
        //for (int column = 0; column < CSVSplitRepositoryData.GetLength(0); column++)
        //{
        //    if (string.IsNullOrEmpty(CSVSplitRepositoryData[column, 1]))
        //    {
        //        continue;
        //    }
        //    else if (uint.Parse(CSVSplitRepositoryData[column, 1]) == eventID)
        //    {
        //        for (int row = 1; targetArray.Length > row; row++)
        //        {
        //            targetArray[row] = CSVSplitRepositoryData[column, row];
        //        }
        //        return targetArray;
        //    }
        //}
        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_SupportCardEventDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_SupportCardEventDataCSV);
    }
}

/// <summary>
/// イベント発生条件
/// </summary>
public struct EventTriggerParamCondition
{
    public RankType PowerRank;
    public RankType IntelligenceRank;
    public RankType PhysicalRank;
    public RankType SpeedRank;
}

