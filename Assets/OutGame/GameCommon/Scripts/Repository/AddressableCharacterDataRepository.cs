using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

/// <summary>
/// キャラクターのデータを取得・保持するクラス
/// </summary>
public class AddressableCharacterDataRepository : RepositoryBase<CharacterBaseDataRegistry>, IAddressableDataRepository, IAsyncStartable
{
    [Inject]
    public AddressableCharacterDataRepository() { }

    public CharacterBaseData GetCharacterData(uint id)
    {
        return _repositoryData.GetData(id);
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();

        if (RepositoryData != null)
            Debug.Log(typeof(CharacterBaseDataRegistry).Name + "のデータのロードに成功しました");
        else
            Debug.Log(typeof(CharacterBaseDataRegistry).Name + "データのロードに失敗しました");
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<CharacterBaseDataRegistry>(AAGCharacterData.kAssets_MasterData_CharacterData_CharacterDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGCharacterData.kAssets_MasterData_CharacterData_CharacterDataRegistry);
    }
}
