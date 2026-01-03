using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

public class AddressableSupportCardImageDataRepository : RepositoryBase<SupportCardImageDataRegistry>, IAddressableDataRepository, IAsyncStartable
{
    [Inject]
    public AddressableSupportCardImageDataRepository() { }

    public Sprite GetSprite(uint id)
    {
        return _repositoryData.GetData(id);
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();

        if (RepositoryData != null)
            Debug.Log(typeof(SupportCardImageDataRegistry).Name + "のデータのロードに成功しました");
        else
            Debug.Log(typeof(SupportCardImageDataRegistry).Name + "データのロードに失敗しました");
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<SupportCardImageDataRegistry>(AAGSupportCardSprite.kAssets_MasterData_ImageData_SupportCardImageDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGSupportCardSprite.kAssets_MasterData_ImageData_SupportCardImageDataRegistry);
    }
}
