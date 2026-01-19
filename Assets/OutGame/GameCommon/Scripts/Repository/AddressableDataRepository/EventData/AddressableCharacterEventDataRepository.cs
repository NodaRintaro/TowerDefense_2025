using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// キャラクター固有イベントのCSVデータのリポジトリ
/// </summary>
public class AddressableCharacterEventDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    [Inject]
    public AddressableCharacterEventDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGEventData.kAssets_MasterData_CSV_EventData_CharacterEventDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGEventData.kAssets_MasterData_CSV_EventData_CharacterEventDataCSV);
    }
}
