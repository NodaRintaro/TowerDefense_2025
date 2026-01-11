using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

/// <summary>
/// キャラクターのデータを取得・保持するクラス
/// </summary>
public class AddressableCharacterDataRepository : RepositoryBase<CharacterBaseDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableCharacterDataRepository() { }

    public CharacterBaseData GetCharacterData(uint id)
    {
        return _repositoryData.GetData(id);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<CharacterBaseDataRegistry>(AAGCharacterData.kAssets_MasterData_CharacterData_CharacterDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGCharacterData.kAssets_MasterData_CharacterData_CharacterDataRegistry);
    }
}
