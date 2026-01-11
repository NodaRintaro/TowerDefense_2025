using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class JsonCharacterCollectionDataRepository : RepositoryBase<CharacterCollectionData>
{
    [Inject]
    public JsonCharacterCollectionDataRepository() { }

    public const string SaveDataName = "JsonCharacterCollection";

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<CharacterCollectionData>(SaveDataName);
    }
}
