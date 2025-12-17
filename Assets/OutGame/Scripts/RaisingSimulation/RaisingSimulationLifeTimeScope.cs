using VContainer.Unity;
using VContainer;
using CharacterData;

public class RaisingSimulationLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TrainingCharacterData>(Lifetime.Singleton);
        builder.Register<TrainingSaveData>(Lifetime.Singleton);

    }
}
