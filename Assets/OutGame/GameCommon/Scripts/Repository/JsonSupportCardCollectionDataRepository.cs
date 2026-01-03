using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class JsonSupportCardCollectionDataRepository : RepositoryBase<SupportCardCollectionData>, IAsyncStartable
{
    [Inject]
    public JsonSupportCardCollectionDataRepository() { }

    public const string SaveDataName = "JsonSupportCardCollection";

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();

        if (RepositoryData != null)
            Debug.Log(typeof(SupportCardCollectionData).Name + "のデータのロードに成功しました");
        else
            Debug.Log(typeof(SupportCardCollectionData).Name + "データのロードに失敗しました");
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<SupportCardCollectionData>(SaveDataName);
    }
}
