using VContainer;
using VContainer.Unity;

/// <summary>
/// データのロードを行うLifeTimeScopeClass
/// </summary>
public class DataLoadManager : LifetimeScope
{
    //説明：
    //RegisterDataRepositoryで必要なデータのRepositoryをRegisterしておいてください
    //そうしたらDataLoadCompleteNotifierがすべてのデータをロードし終わったことを通知します

    //取得方法：
    //取得したいクラス名 Class =　シーン上から取ってきたDataLoadManagerのインスタンス.Container.Resolve<取得したいクラス名>();

    //注意！：
    //データのLoadが完了する前にデータを取得しようとするとエラーが出ます
    //必ずDataLoadCompleteNotifierのデータのロード終了通知を待ってから取得してください

    protected override void Configure(IContainerBuilder builder)
    {
        //データリポジトリ必要なデータリポジトリを全てContainerに登録する処理
        RegisterDataRepository(builder);

        //全てのデータリポジトリのロードを管理し、完了を通知するクラス
        builder.Register<RepositoryDataLoader>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //全てのデータリポジトリのロードが完了したことを通知するクラス
        builder.RegisterComponentOnNewGameObject<DataLoadCompleteNotifier>(Lifetime.Singleton);
    }

    /// <summary> RepositoryClassをContainer登録する </summary>
    private void RegisterDataRepository(IContainerBuilder builder)
    {
        //キャラクターの所有データのRepository
        builder.Register<JsonCharacterCollectionDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //サポートカードの所有データのRepository
        builder.Register<JsonSupportCardCollectionDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //トレーニング対象データのRepository
        builder.Register<JsonTrainingSaveDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //キャラクターデータのRepository
        builder.Register<AddressableCharacterDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //サポートカードデータのRepository
        builder.Register<AddressableSupportCardDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //キャラクターの画像データのRepository
        builder.Register<AddressableCharacterImageDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //サポートカードの画像データのRepository
        builder.Register<AddressableSupportCardImageDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //各ランクの画像データのRepository
        builder.Register<AddressableRankImageDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //キャラクターの職種の画像データのRepository
        builder.Register<AddressableCharacterJobImageDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //ノベルデータのRepository
        builder.Register<AddressableNovelEventDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //トレーニングイベントデータのRepository
        builder.Register<AddressableTrainingEventDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //分岐先のイベントデータのRepository
        builder.Register<AddressableBranchTrainingEventDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
    }
}
