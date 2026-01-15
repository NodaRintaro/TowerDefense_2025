using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

public class AddressableNovelEventDataRepository : RepositoryBase<NovelEventDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableNovelEventDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<NovelEventDataRegistry>(AAGNovelData.kAssets_MasterData_NovelData_NovelEventData);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGNovelData.kAssets_MasterData_NovelData_NovelEventData);
    }
}
