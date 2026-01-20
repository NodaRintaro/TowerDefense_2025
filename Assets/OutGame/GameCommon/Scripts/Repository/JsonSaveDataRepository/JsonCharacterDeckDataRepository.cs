using Cysharp.Threading.Tasks;
using TowerDefenseDeckData;
using VContainer;
using System.Threading;

public class JsonCharacterDeckDataRepository : RepositoryBase<CharacterDeckDataBase>
{
    [Inject]
    public JsonCharacterDeckDataRepository() { }

    public const string SaveDataName = "JsonCharacterDeckData";

    public async UniTask DataSaveAsync()
    {
        await JsonDataSaveSystem.DataSaveAsync(_repositoryData, SaveDataName);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<CharacterDeckDataBase>(SaveDataName);
    }
}
