using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class TrainingDataSelectLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //トレーニングデータの生成クラス
        builder.Register<TrainingDataGenerater>(Lifetime.Singleton);
    }
}