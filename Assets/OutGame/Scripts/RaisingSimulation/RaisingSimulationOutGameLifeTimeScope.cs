using UnityEngine;
using VContainer;
using VContainer.Unity;

/// <summary>
/// キャラクター育成のキャラクター、サポートカードの選択画面で使用するLifeTimeScope
/// </summary>
public sealed class RaisingSimulationOutGameLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //キャラクター、サポートカードのGameObjectを生成するクラス
        builder.Register<ButtonUIGenerater>(Lifetime.Singleton);

        //表示画面の切り替えクラス
        builder.Register<ScreenManager>(Lifetime.Singleton);
    }
}