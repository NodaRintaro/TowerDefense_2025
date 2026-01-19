using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// 共通のトレーニングイベントのCSVデータのリポジトリ
/// </summary>
public class AddressableTrainingEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    [Inject]
    public AddressableTrainingEventDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_TrainingEventDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_TrainingEventDataCSV);
    }
}
