using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Threading;

/// <summary>
/// キャラクターのデータを取得・保持するクラス
/// </summary>
public class AddressableCharacterDataRepository : RepositoryBase<CharacterBaseDataRegistry>, IAddressableDataRepository
{
    [Inject]
    public AddressableCharacterDataRepository()
    {

    }

    public CharacterBaseData GetCharacterDataByID(uint id)
    {
        return _repositoryData.GetData(id);
    }

    public CharacterBaseData GetCharacterDataByName(string characterName)
    {
        foreach(var data in _repositoryData.DataHolder)
        {
            if(data.CharacterName == characterName) return data;
        }
        return null;
    }

    public override async UniTask DataLoadAsync(CancellationToken cancellation)
    {
        _repositoryData = await AssetsLoader.LoadAssetAsync<CharacterBaseDataRegistry>(AAGCharacterData.kAssets_MasterData_ScriptableObject_CharacterData_CharacterDataRegistry);
    }

    public void DataRelease()
    {
        AssetsLoader.Release(AAGCharacterData.kAssets_MasterData_ScriptableObject_CharacterData_CharacterDataRegistry);
    }
}
