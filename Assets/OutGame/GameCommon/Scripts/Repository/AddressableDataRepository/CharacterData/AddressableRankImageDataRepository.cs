using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

/// <summary>
/// 各ランクの画像データを取得・保持するクラス
/// </summary>
public class AddressableRankImageDataRepository : RepositoryBase<RankImageDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableRankImageDataRepository() { }

    public Sprite GetSprite(RankType rankType)
    {
        return _repositoryData.GetData(rankType);
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<RankImageDataRegistry>(AAGRankSprite.kAssets_MasterData_ScriptableObject_ImageData_RankImageDataRegistry);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGRankSprite.kAssets_MasterData_ScriptableObject_ImageData_RankImageDataRegistry);
    }
}
