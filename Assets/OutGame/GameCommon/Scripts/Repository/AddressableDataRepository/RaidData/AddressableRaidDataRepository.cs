using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

public class AddressableRaidDataRepository :RepositoryBase<RaidDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableRaidDataRepository() { }
    
    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        //_repositoryData = await AssetsLoader.LoadAssetAsync<RaidDataRegistry>(AAGRaidData.kAssets_MasterData_ScriptableObject_CharacterEventSchedule_RaidData);
    }
    public void DataRelease()
    {
        //AssetsLoader.Release(AAGRaidData.kAssets_MasterData_ScriptableObject_CharacterEventSchedule_RaidData);

    }

}