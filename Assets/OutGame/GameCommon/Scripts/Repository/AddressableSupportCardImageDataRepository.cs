using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using System.Threading;

public class AddressableSupportCardImageDataRepository : RepositoryBase<SupportCardImageDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableSupportCardImageDataRepository() { }

    public Sprite GetSprite(uint id)
    {
        return _repositoryData.GetData(id);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<SupportCardImageDataRegistry>(AAGSupportCardSprite.kAssets_MasterData_ImageData_SupportCardImageDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGSupportCardSprite.kAssets_MasterData_ImageData_SupportCardImageDataRegistry);
    }
}
