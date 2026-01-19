using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

public class AddressableCharacterTrainingScheduleRepository : RepositoryBase<CharacterTrainingScheduleRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableCharacterTrainingScheduleRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<CharacterTrainingScheduleRegistry>(AAGCharacterTrainingSchedule.kAssets_MasterData_ScriptableObject_CharacterEventSchedule_CharacterTrainingSchedule);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGCharacterTrainingSchedule.kAssets_MasterData_ScriptableObject_CharacterEventSchedule_CharacterTrainingSchedule);
    }
}
