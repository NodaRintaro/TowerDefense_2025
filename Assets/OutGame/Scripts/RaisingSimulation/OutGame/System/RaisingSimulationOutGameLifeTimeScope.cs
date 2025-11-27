using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class RaisingSimulationOutGameLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //トレーニングデータの生成クラス
        builder.Register<TrainingDataCreater>(Lifetime.Singleton);

        //表示画面の切り替えクラス
        builder.Register<ScreenChanger>(Lifetime.Singleton);
    }
}