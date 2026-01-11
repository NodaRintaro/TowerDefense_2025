using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

/// <summary>
/// 全てのデータリポジトリのロードを管理し、完了を通知するクラス
/// </summary>
public class RepositoryDataLoader : IAsyncStartable
{
    private readonly IEnumerable<IAsyncDataLoader> _asyncDataLoader;
    private readonly DataLoadCompleteNotifier _dataLoadCompleteNotifier;

    [Inject]
    public RepositoryDataLoader(IEnumerable<IAsyncDataLoader> asyncDataLoader, DataLoadCompleteNotifier dataLoadCompleteNotifier)
    {
        _asyncDataLoader = asyncDataLoader;
        _dataLoadCompleteNotifier = dataLoadCompleteNotifier;
    }

    /// <summary>
    /// LifeTimeScope生成時に呼び出され全てのLoadが終了したことを通知する
    /// </summary>
    public async UniTask StartAsync(CancellationToken cancellation)
    {
        var tasks = _asyncDataLoader.Select(x => x.DataLoadAsync(cancellation));
        await UniTask.WhenAll(tasks);

        _dataLoadCompleteNotifier.NotifyDataLoadComplete();
    }
}