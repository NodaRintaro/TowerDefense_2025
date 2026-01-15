using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

public class AddressableBranchTrainingEventDataRepository : RepositoryBase<BranchTrainingEventDataRegistry>
{
    [Inject]
    public AddressableBranchTrainingEventDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<BranchTrainingEventDataRegistry>(AAGTrainingEventData.kAssets_MasterData_TrainingEventData_BranchTrainingEventData);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGTrainingEventData.kAssets_MasterData_TrainingEventData_BranchTrainingEventData);
    }
}
