using RaisingSimulationGameFlowStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class HomeMenuLifeTimeScope : LifetimeScope
{
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

        //キャラクターの画像データのRepository
        builder.Register<AddressableCharacterImageDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        //現在のキャラクターデータベース
        builder.Register<TowerDefenseCharacterDataBase>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        
        //キャラクターベースデータのRegistry
        builder.Register<AddressableCharacterDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        
        //ランク画像データのRepository
        builder.Register<AddressableRankImageDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        
        //職業画像データのRepository
        builder.Register<AddressableCharacterJobImageDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        
        //キャラクター編成データのRepository
        builder.Register<JsonCharacterDeckDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        
        //ステージデータのRepository
        builder.Register<AddressableStageDataRepository>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
    }
}
