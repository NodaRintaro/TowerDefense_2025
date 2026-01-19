using Cysharp.Threading.Tasks;
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
    [Inject]
    public AddressableSupportCardEventScenarioDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_SupportCardScenarioDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_SupportCardScenarioDataCSV);
    }
}
