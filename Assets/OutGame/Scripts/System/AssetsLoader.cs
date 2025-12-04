using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Assetをロードするためのクラス
/// </summary>
public static class AssetsLoader
{
    private readonly static Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();

    public static async UniTask<T> LoadAssetAsync<T>(string address)
    {
        AsyncOperationHandle<T> addressableHandle = default;
        if (!_handles.ContainsKey(address))
        {
            addressableHandle = Addressables.LoadAssetAsync<T>(address);
            _handles.Add(address, addressableHandle);
        }
        else
        {
            return (T)_handles[address].Result;
        }
        await addressableHandle.Task;
        return addressableHandle.Result;
    }

    /// <summary>
    ///　リソースを開放する
    /// </summary>      
    /// <param name="address"></param>
    public static void Release(string address)
    {
        Addressables.Release(_handles[address]);
        _handles.Remove(address);
    }
}