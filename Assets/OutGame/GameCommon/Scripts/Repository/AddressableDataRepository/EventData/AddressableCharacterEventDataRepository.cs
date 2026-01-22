using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// キャラクター固有イベントのCSVデータのリポジトリ
/// </summary>
public class AddressableCharacterEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository, ICSVDataRepository
{
    public string[,] CSVSplitRepositoryData => CSVLoader.LoadCsv(_repositoryData);

    [Inject]
    public AddressableCharacterEventDataRepository() { }

    public string[] GetCsvEventData(uint characterID, uint eventID)
    {
        bool isFindCharacterID = false;
        string[] targetArray = new string[CSVSplitRepositoryData.GetLength(1)];
        for (int column = 0; column < CSVSplitRepositoryData.GetLength(0); column++)
        {
            if (!isFindCharacterID)
            {
                if (string.IsNullOrEmpty(CSVSplitRepositoryData[column, 0]))
                {
                    continue;
                }
                else if (uint.Parse(CSVSplitRepositoryData[column, 0]) == characterID)
                {
                    isFindCharacterID = true;
                    continue;
                }
            }
            else
            {
                if (uint.Parse(CSVSplitRepositoryData[column, 1]) == eventID)
                {
                    for (int row = 1; targetArray.Length > row; row++)
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
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_CharacterEventDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_CharacterEventDataCSV);
    }
}
