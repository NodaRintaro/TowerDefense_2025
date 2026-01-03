using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

/// <summary>
/// キャラクターの職種データを取得・保持するクラス
/// </summary>
public class AddressableCharacterJobImageDataRepository : RepositoryBase<CharacterJobImageDataRegistry>, IAddressableDataRepository, IAsyncStartable
{
    [Inject]
    public AddressableCharacterJobImageDataRepository() { }

    public Sprite GetSprite(JobType jobType)
    {
        return _repositoryData.GetData(jobType);
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await DataLoadAsync();

        if (RepositoryData != null)
            Debug.Log(typeof(CharacterJobImageDataRegistry).Name + "のデータのロードに成功しました");
        else
            Debug.Log(typeof(CharacterJobImageDataRegistry).Name + "データのロードに失敗しました");
    }

    public override async UniTask DataLoadAsync()
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<CharacterJobImageDataRegistry>(AAGJobSprite.kAssets_MasterData_ImageData_CharacterJobImageDataRegistry);
        DataRelease();
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGJobSprite.kAssets_MasterData_ImageData_CharacterJobImageDataRegistry);
    }
}
