using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// キャラクター固有イベントのCSVデータのリポジトリ
/// </summary>
public class AddressableCharacterEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    private string[,] _csvSplitRepositoryData = null;

    [Inject]
    public AddressableCharacterEventDataRepository() 
    {
        
    }

    public TrainingEventData GetCsvEventData(uint characterID, uint eventID)
    {
        Debug.Log("データの検索を開始");
        Debug.Log($"{characterID} {eventID}");
        bool isFindCharacterID = false;
        string[] targetArray = new string[_csvSplitRepositoryData.GetLength(1) - 1];
        for (int column = 1; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if (!isFindCharacterID)
            {

                if (uint.TryParse(_csvSplitRepositoryData[column, 0], out uint result) && result == characterID)
                {
                    isFindCharacterID = true;
                    Debug.Log("キャラクターIDを発見");
                    continue; 
                }

                continue;
            }
            else
            {
                if(!string.IsNullOrEmpty(_csvSplitRepositoryData[column, 0]) && 
                    uint.TryParse(_csvSplitRepositoryData[column, 0], out uint parsedCharId) && parsedCharId != characterID)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(_csvSplitRepositoryData[column, 1]))
                {
                    continue;
                }

                if (uint.TryParse(_csvSplitRepositoryData[column, 1], out uint parsedEventId) && parsedEventId == eventID)
                {
                    const int startArrNum = 1;
                    for (int row = startArrNum; row < targetArray.Length; row++)
                    {
                        targetArray[row - startArrNum] = _csvSplitRepositoryData[column, row];
                    }
                    return TrainingEventDataGenerator.GenerateEventData(targetArray);
                }
            }
        }

        Debug.Log("データが見つかりませんでした");
        return null;
    }

    public List<TrainingEventData> GetBranchEventData(uint characterID, uint eventID)
    {
        bool isFindCharacterID = false;
        string[] targetArray = new string[_csvSplitRepositoryData.GetLength(1)];
        for (int column = 1; column < _csvSplitRepositoryData.GetLength(0); column++)
        {
            if (!isFindCharacterID)
            {
                if (string.IsNullOrEmpty(_csvSplitRepositoryData[column, 0]))
                {
                    continue;
                }
                else if (uint.Parse(_csvSplitRepositoryData[column, 0]) == characterID)
                {   
                    isFindCharacterID = true;
                    continue;
                }
            }
            else
            {
                if (uint.Parse(_csvSplitRepositoryData[column, 1]) == eventID)
                {
                    List <TrainingEventData> targetList = new List <TrainingEventData>();
                    int branchEventCount = 1;
                    while(string.IsNullOrEmpty(_csvSplitRepositoryData[column + branchEventCount, 1]))
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
        }

        Debug.Log("データが見つかりませんでした");
        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_CharacterEventDataCSV);
        _csvSplitRepositoryData = CSVLoader.LoadCsv(_repositoryData);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_CharacterEventDataCSV);
    }
}
