using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

public class AddressableSupportCardDataRepository : RepositoryBase<SupportCardDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableSupportCardDataRepository() { }

    public SupportCardData GetSupportCardData(uint id)
    {
        return _repositoryData.GetData(id);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<SupportCardDataRegistry>(AAGSupportCardData.kAssets_MasterData_SupportCard_SupportCardDataRegistry);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGSupportCardData.kAssets_MasterData_SupportCard_SupportCardDataRegistry);
    }
}
