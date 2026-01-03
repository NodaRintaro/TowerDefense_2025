using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

public class AddressableCharacterImageDataRepository : RepositoryBase<CharacterImageDataRegistry>, IAddressableDataRepository, IAsyncStartable
{
    [Inject]
    public AddressableCharacterImageDataRepository() { }

    public CharacterImageData GetImageData(uint id)
    {
        return _repositoryData.GetImageData(id);
    }

    public Sprite GetSprite(uint id, CharacterSpriteType characterSpriteType)
    {
        return _repositoryData.GetCharacterSprite(id, characterSpriteType);
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();

        if (RepositoryData != null)
            Debug.Log(typeof(CharacterImageDataRegistry).Name + "のデータのロードに成功しました");
        else
            Debug.Log(typeof(CharacterImageDataRegistry).Name + "データのロードに失敗しました");
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<CharacterImageDataRegistry>(AAGCharacterSprite.kAssets_MasterData_ImageData_CharacterImageDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGCharacterSprite.kAssets_MasterData_ImageData_CharacterImageDataRegistry);
    }
}
