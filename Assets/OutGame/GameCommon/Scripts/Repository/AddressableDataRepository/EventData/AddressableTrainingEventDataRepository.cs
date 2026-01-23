using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;
using System;

/// <summary>
/// 共通のトレーニングイベントのCSVデータのリポジトリ
/// </summary>
public class AddressableTrainingEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    private string[,] _csvSplitRepositoryData = null;

    [Inject]
    public AddressableTrainingEventDataRepository() { }

    /// <summary> トレーニングイベントの取得 </summary>
    public TrainingEventData GetCsvEventData(uint eventID)
    {
        for (int column = 1; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if (uint.TryParse(_csvSplitRepositoryData[column, 0], out uint parsedEventId) && parsedEventId == eventID)
            {
                int actualDataLength = _csvSplitRepositoryData.GetLength(1);

                if (actualDataLength < 0) actualDataLength = 0;

                string[] eventDataArray = new string[actualDataLength]; 
                for (int i = 0; i < actualDataLength; i++)
                {
                    eventDataArray[i] = _csvSplitRepositoryData[column, i];
                }
                return TrainingEventDataGenerator.GenerateEventData(eventDataArray);
            }
        }

        Debug.Log("データが見つかりませんでした");
        return null;
    }

    /// <summary> トレーニング分岐イベントの取得 </summary>
    public List<TrainingEventData> GetBranchEventData(uint eventID)
    {
        List <TrainingEventData> targetList = new List < TrainingEventData >();

        bool isTargetEventBranch = false;
        for (int column = 1; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if (!isTargetEventBranch)
            {
                if (uint.TryParse(_csvSplitRepositoryData[column, 0], out uint parsedEventId) && parsedEventId == eventID)
                {
                    isTargetEventBranch = true;
                }
            }
            else
            {
                if(uint.TryParse(_csvSplitRepositoryData[column, 0], out uint parsedEventId))
                    return targetList;


                int actualDataLength = _csvSplitRepositoryData.GetLength(1);

                if (actualDataLength < 0) actualDataLength = 0;

                string[] eventDataArray = new string[actualDataLength];
                for (int i = 0; i < actualDataLength; i++)
                {
                    eventDataArray[i] = _csvSplitRepositoryData[column, i];
                }
                targetList.Add(TrainingEventDataGenerator.GenerateEventData(eventDataArray));
            }
        }
        return targetList;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_TrainingCommonEventDataCSV);
        _csvSplitRepositoryData = CSVLoader.LoadCsv(_repositoryData);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_TrainingCommonEventDataCSV);
    }
}
