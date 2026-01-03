using Cysharp.Threading.Tasks;
using TowerDefenseDeckData;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

public class JsonCharacterDeckDataRepository : RepositoryBase<CharacterDeckDataRegistry>, IAsyncStartable
{
    [Inject]
    public JsonCharacterDeckDataRepository() { }

    public const string SaveDataName = "JsonCharacterDeckData";

    public async UniTask DataSave()
    {
        await JsonDataSaveSystem.DataSave(_repositoryData, SaveDataName);
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();

        if (RepositoryData != null)
            Debug.Log(typeof(CharacterDeckDataRegistry).Name + "のデータのロードに成功しました");
        else
            Debug.Log(typeof(CharacterDeckDataRegistry).Name + "データのロードに失敗しました");
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<CharacterDeckDataRegistry>(SaveDataName);
    }
}
