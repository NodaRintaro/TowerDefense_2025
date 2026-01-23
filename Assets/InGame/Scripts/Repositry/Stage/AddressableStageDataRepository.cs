using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

public class AddressableStageDataRepository : RepositoryBase<StageDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableStageDataRepository() { }
    
    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<StageDataRegistry>(AAGStageData.kAssets_MasterData_ScriptableObject_StageData);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGStageData.kAssets_MasterData_ScriptableObject_StageData);
    }
}
