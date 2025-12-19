using VContainer.Unity;
using VContainer;
using CharacterData;

public class RaisingSimulationLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TrainingTargetSaveData>(Lifetime.Singleton);

        builder.Register<RaisingSimulationScreenPresenter>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<TrainingCharacterSelectionPresenter>();
    }
}
