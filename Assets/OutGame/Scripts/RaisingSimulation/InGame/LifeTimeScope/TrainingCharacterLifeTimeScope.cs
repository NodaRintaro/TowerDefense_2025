using VContainer.Unity;
using VContainer;

public class TrainingCharacterLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TrainingCharacterData>(Lifetime.Singleton);
        builder.Register<TrainingSaveDataManager>(Lifetime.Singleton);
        builder.Register<TrainingDataHolder>(Lifetime.Singleton);
        builder.Register<ParameterRankSelecter>(Lifetime.Singleton);

    }
}
