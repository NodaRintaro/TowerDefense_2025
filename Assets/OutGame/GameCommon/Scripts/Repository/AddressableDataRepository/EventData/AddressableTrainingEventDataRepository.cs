using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;
using System;

/// <summary>
/// 共通のトレーニングイベントのCSVデータのリポジトリ
/// </summary>
public class AddressableTrainingEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository, ICSVDataRepository
{
    public string[,] CSVSplitRepositoryData => CSVLoader.LoadCsv(_repositoryData);

    [Inject]
    public AddressableTrainingEventDataRepository() { }

    public string[] GetCsvEventData(uint id)
    {
        string[] targetArray = new string[CSVSplitRepositoryData.GetLength(1)];
        for (int column = 0; column < CSVSplitRepositoryData.GetLength(0); column++)
        {
            if(string.IsNullOrEmpty(CSVSplitRepositoryData[column, 0]))
            {
                continue;
            }
            else
            {
                if(uint.Parse(CSVSplitRepositoryData[column, 0]) == id)
                {
                    for(int row = 0; targetArray.Length > row ; row++)
                    {
                        targetArray[row] = CSVSplitRepositoryData[column, row];
                    }
                    return targetArray;
                }
            }
        }
        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_TrainingEventDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_TrainingEventDataCSV);
    }
}
