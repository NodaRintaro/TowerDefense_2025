using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

public class AddressableTrainingEventDataRepository : RepositoryBase<TrainingEventDataRegistry>
{
    [Inject]
    public AddressableTrainingEventDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TrainingEventDataRegistry>(AAGTrainingEventData.kAssets_MasterData_TrainingEventData_TrainingEventData);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGTrainingEventData.kAssets_MasterData_TrainingEventData_TrainingEventData);
    }
}
