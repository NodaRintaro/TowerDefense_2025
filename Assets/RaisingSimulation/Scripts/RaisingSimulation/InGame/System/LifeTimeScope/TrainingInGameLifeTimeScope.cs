using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using VContainer;

public class TrainingInGameLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TrainingDataHolder>(Lifetime.Singleton);
        builder.Register<TrainingDataManager>(Lifetime.Singleton);
    }
}
