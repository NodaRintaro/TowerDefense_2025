using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class JsonTrainingTargetSaveDataRepository : RepositoryBase<TrainingTargetSaveData>, IAsyncStartable
{
    [Inject]
    public JsonTrainingTargetSaveDataRepository() { }

    public const string SaveDataName = "JsonTrainingTargetSaveData";

    public async UniTask DataSave()
    {
        await JsonDataSaveSystem.DataSave(_repositoryData, SaveDataName);
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<TrainingTargetSaveData>(SaveDataName);
    }

}
