using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

/// <summary>
/// キャラクターの職種データを取得・保持するクラス
/// </summary>
public class AddressableCharacterJobImageDataRepository : RepositoryBase<CharacterJobImageDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableCharacterJobImageDataRepository() { }

    public Sprite GetSprite(JobType jobType)
    {
        return _repositoryData.GetData(jobType);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<CharacterJobImageDataRegistry>(AAGJobSprite.kAssets_MasterData_ImageData_CharacterJobImageDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGJobSprite.kAssets_MasterData_ImageData_CharacterJobImageDataRegistry);
    }
}
