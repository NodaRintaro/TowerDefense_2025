using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

/// <summary>
/// データ管理用RepositoryのBaseクラス
/// </summary>
/// <typeparam name="TData"></typeparam>
public abstract class RepositoryBase<TData> : IAsyncDataLoader
{
    protected TData _repositoryData;

    public TData RepositoryData => _repositoryData;

    public void SetData(TData data)
    {
        _repositoryData = data;
    }

    /// <summary> 対象のデータをロードする </summary>
    public abstract UniTask DataLoadAsync(CancellationToken cancellation);
}