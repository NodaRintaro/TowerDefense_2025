using VContainer.Unity;
using VContainer;
using TrainingData;

public class RaisingSimulationInGameLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TrainingCharacterData>(Lifetime.Singleton);
        builder.Register<TrainingDataHolder>(Lifetime.Singleton);

    }
}
