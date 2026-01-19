using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// キャラクター固有イベントのシナリオのCSVデータのリポジトリ
/// </summary>
public class AddressableCharacterEventScenarioDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    [Inject]
    public AddressableCharacterEventScenarioDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_CharacterEventScenarioDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_CharacterEventScenarioDataCSV);
    }
}
