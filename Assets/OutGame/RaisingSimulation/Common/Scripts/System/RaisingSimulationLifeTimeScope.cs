using VContainer;
using VContainer.Unity;
using RaisingSimulationGameFlowStateMachine;

/// <summary> 育成ゲーム全体のLifeTimeScope </summary>
public class RaisingSimulationLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //データリポジトリ必要なデータリポジトリを全てContainerに登録する処理
        RegisterDataRepository(builder);

        //全てのデータリポジトリのロードを管理し、完了を通知するクラス
        builder.Register<RepositoryDataLoader>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();

        //全てのデータリポジトリのロードが完了したことを通知するクラス
        builder.RegisterComponentOnNewGameObject<DataLoadCompleteNotifier>(Lifetime.Singleton);

        //育成ゲームのゲーム全体の進行管理役のステートマシン
        builder.RegisterComponentInHierarchy<GameFlowStateMachine>();

        //育成ゲームのイベント登録用Pool
        builder.Register<TrainingEventPool>(Lifetime.Singleton);

        //育成イベントのキャラクターへのバフ計算Class
        builder.Register<ParameterBuffCalculator>(Lifetime.Singleton);

        //トレーニングイベントの生成を行うクラス
        builder.Register<TrainingEventDataGenerator>(Lifetime.Singleton);

        //一日で行われる育成イベントを決めるクラス
        builder.Register<TrainingEventSelector>(Lifetime.Singleton);
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

        //トレーニングイベントのノベルデータのRepository
        builder.Register<AddressableTrainingEventScenarioDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //トレーニングイベントデータのRepository
        builder.Register<AddressableTrainingEventDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        
        //キャラクター固有トレーニングスケジュールデータのRepository
        builder.Register<AddressableCharacterTrainingScheduleRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //キャラクター固有イベントデータのRepository
        builder.Register<AddressableCharacterEventDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //キャラクター固有シナリオデータのRepository
        builder.Register<AddressableCharacterEventScenarioDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //サポートカード固有イベントデータのRepository
        builder.Register<AddressableSupportCardEventDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //サポートカードシナリオデータのRepository
        builder.Register<AddressableSupportCardEventScenarioDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
    }
}
