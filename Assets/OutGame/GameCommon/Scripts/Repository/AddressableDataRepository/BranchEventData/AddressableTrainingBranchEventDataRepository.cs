using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// トレーニングの分岐イベントのCSVデータリポジトリ
/// </summary>
public class AddressableTrainingBranchEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    [Inject]
    public AddressableTrainingBranchEventDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_TrainingCommonEventBranchEventDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_TrainingCommonEventBranchEventDataCSV);
    }
}
