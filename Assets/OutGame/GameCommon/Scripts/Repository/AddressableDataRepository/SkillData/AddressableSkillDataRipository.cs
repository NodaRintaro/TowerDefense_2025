using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

public class AddressableSkillDataRipository : RepositoryBase<SkillDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableSkillDataRipository() { }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<SkillDataRegistry>(AAGSkillData.kAssets_MasterData_ScriptableObject_SkillData_SkillData);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGSkillData.kAssets_MasterData_ScriptableObject_SkillData_SkillData);
    }
}
