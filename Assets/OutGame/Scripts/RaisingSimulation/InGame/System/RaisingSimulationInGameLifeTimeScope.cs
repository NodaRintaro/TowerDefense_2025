using VContainer.Unity;
using VContainer;

public class RaisingSimulationInGameLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TrainingCharacterData>(Lifetime.Singleton);
        builder.Register<TrainingDataHolder>(Lifetime.Singleton);

    }
}
