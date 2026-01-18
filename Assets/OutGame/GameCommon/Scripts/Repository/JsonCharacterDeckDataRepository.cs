using Cysharp.Threading.Tasks;
using TowerDefenseDeckData;
using VContainer;
using System.Threading;

public class JsonCharacterDeckDataRepository : RepositoryBase<CharacterDeckDataRegistry>
{
    [Inject]
    public JsonCharacterDeckDataRepository() { }

    public const string SaveDataName = "JsonCharacterDeckData";

    public async UniTask DataSave()
    {
        await JsonDataSaveSystem.DataSave(_repositoryData, SaveDataName);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<CharacterDeckDataRegistry>(SaveDataName);
    }
}
