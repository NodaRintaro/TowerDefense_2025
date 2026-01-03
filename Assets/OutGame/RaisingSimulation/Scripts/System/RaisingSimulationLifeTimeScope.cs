using VContainer;
using VContainer.Unity;

/// <summary> 育成ゲーム全体のLifeTimeScope </summary>
public class RaisingSimulationLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        RegisterDataRepository(builder);

        builder.RegisterEntryPoint<CharacterSelectInitializer>(Lifetime.Singleton);

        builder.Register<ScreenChanger>(Lifetime.Singleton);
    }

    /// <summary> RepositoryClassをContainer登録する </summary>
    private void RegisterDataRepository(IContainerBuilder builder)
    {
        //キャラクターの所有データのRepository
        builder.Register<JsonCharacterCollectionDataRepository>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //サポートカードの所有データのRepositry
        builder.Register<JsonSupportCardCollectionDataRepository>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //トレーニング対象データのRepository
        builder.Register<JsonTrainingTargetSaveDataRepository>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //キャラクターデータのRepository
        builder.Register<AddressableCharacterDataRepository>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //サポートカードデータのRepository
        builder.Register<AddressableSupportCardRepository>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //キャラクターの画像データのRepository
        builder.Register<AddressableCharacterImageDataRepository>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //各ランクの画像データのRepository
        builder.Register<AddressableRankImageDataRepository>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //キャラクターの職種の画像データのRepository
        builder.Register<AddressableCharacterJobImageDataRepository>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();
    }
}
