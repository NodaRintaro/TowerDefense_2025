using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

/// <summary>
/// トレーニングイベントのシナリオのCSVデータのリポジトリ
/// </summary>
public class AddressableTrainingEventScenarioDataRepository : RepositoryBase<TextAsset>, IAddressableDataRepository
{
    [Inject]
    public AddressableTrainingEventScenarioDataRepository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<TextAsset>(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_TrainingCommonEventScenarioDataCSV);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGScenarioData.kAssets_MasterData_CSV_ScenarioData_TrainingCommonEventScenarioDataCSV);
    }
}
