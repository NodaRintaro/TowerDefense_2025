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
    [Inject]
    public AddressableSupportCardEventDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_SupportCardEventDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_SupportCardEventDataCSV);
    }
}
