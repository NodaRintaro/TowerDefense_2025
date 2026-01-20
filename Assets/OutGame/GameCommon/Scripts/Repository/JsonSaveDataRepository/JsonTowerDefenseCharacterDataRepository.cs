using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

public class JsonTowerDefenseCharacterDataRepository : RepositoryBase<TowerDefenseCharacterDataBase>
{
    [Inject]
    public JsonTowerDefenseCharacterDataRepository() { }

    public const string SaveDataName = "JsonTowerDefenseCharacterData";

    public async UniTask DataSave()
    {
        await JsonDataSaveSystem.DataSaveAsync(_repositoryData, SaveDataName);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await JsonDataSaveSystem.DataLoadAsync<TowerDefenseCharacterDataBase>(SaveDataName);

        if (_repositoryData == null)
        {
            _repositoryData = new TowerDefenseCharacterDataBase();
        }
    }

}
