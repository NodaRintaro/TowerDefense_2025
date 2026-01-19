using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class JsonSupportCardCollectionDataRepository : RepositoryBase<SupportCardCollectionData>
{
    [Inject]
    public JsonSupportCardCollectionDataRepository() { }

    public const string SaveDataName = "JsonSupportCardCollection";

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<SupportCardCollectionData>(SaveDataName);
    }
}
