using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class JsonCharacterCollectionDataRepository : RepositoryBase<CharacterCollectionData>, IAsyncStartable
{
    [Inject]
    public JsonCharacterCollectionDataRepository() { }

    public const string SaveDataName = "JsonCharacterCollection";

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<CharacterCollectionData>(SaveDataName);
    }
}
