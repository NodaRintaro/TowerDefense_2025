using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class TrainingDataSelectLifeTimeScope : LifetimeScope
{
    [SerializeField,Header("SceneLoad後に最初に表示されるUI名")] 
    private string _firstScreenName;

    private const string _screenUITag = "ScreenUI";

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<SupportCardDataBase>(Lifetime.Singleton);
        builder.Register<SceneChanger>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<CharacterPickController>();
        builder.RegisterComponentInHierarchy<SupportCardSelectController>();
        builder.Register<SupportCardSelecter>(Lifetime.Singleton);
        builder.Register<ScreenChanger>(Lifetime.Singleton);
    }
}