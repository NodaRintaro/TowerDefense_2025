using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class JsonTrainingSaveDataRepository : RepositoryBase<TrainingData>
{
    [Inject]
    public JsonTrainingSaveDataRepository() { }

    public const string SaveDataName = "JsonTrainingTargetSaveData";

    public async UniTask DataSave()
    {
        await JsonDataSaveSystem.DataSave(_repositoryData, SaveDataName);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<TrainingData>(SaveDataName);

        if(_repositoryData == null)
        {
            _repositoryData = new TrainingData();
        }
    }
}
