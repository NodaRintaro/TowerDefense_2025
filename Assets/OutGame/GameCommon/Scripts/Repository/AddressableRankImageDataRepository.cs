using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

/// <summary>
/// 各ランクの画像データを取得・保持するクラス
/// </summary>
public class AddressableRankImageDataRepository : RepositoryBase<RankImageDataRegistry>, IAddressableDataRepository, IAsyncStartable
{
    [Inject]
    public AddressableRankImageDataRepository() { }

    public Sprite GetSprite(RankType rankType)
    {
        return _repositoryData.GetData(rankType);
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();

        if (RepositoryData != null)
            Debug.Log(typeof(RankImageDataRegistry).Name + "のデータのロードに成功しました");
        else
            Debug.Log(typeof(RankImageDataRegistry).Name + "データのロードに失敗しました");
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<RankImageDataRegistry>(AAGRankSprite.kAssets_MasterData_ImageData_RankImageDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGRankSprite.kAssets_MasterData_ImageData_RankImageDataRegistry);
    }
}
