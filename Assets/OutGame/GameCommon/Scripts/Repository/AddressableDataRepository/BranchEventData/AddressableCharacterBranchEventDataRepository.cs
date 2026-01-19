using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// キャラクター固有の分岐イベントのCSVデータリポジトリ
/// </summary>
public class AddressableCharacterBranchEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    [Inject]
    public AddressableCharacterBranchEventDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_CharacterBranchEventDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_CharacterBranchEventDataCSV);
    }
}
