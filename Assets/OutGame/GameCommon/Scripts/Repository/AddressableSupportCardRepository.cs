using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

public class AddressableSupportCardRepository : RepositoryBase<SupportCardDataRegistry>, IAddressableDataRepository, IAsyncStartable
{
    [Inject]
    public AddressableSupportCardRepository() { }

    public SupportCardData GetSupportCardData(uint id)
    {
        return _repositoryData.GetData(id);
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();

        if (RepositoryData != null)
            Debug.Log(typeof(SupportCardDataRegistry).Name + "のデータのロードに成功しました");
        else
            Debug.Log(typeof(SupportCardDataRegistry).Name + "データのロードに失敗しました");
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<SupportCardDataRegistry>(AAGSupportCardData.kAssets_MasterData_SupportCard_SupportCardDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGSupportCardData.kAssets_MasterData_SupportCard_SupportCardDataRegistry);
    }
}
