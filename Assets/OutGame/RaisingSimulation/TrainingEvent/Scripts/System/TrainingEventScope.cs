using RaisingSimulationGameFlowStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TrainingEventScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //トレーニングイベントの段階管理を行うステートマシン
        builder.RegisterComponentInHierarchy<TrainingEventStateMachine>();

        //育成イベントのキャラクターへのバフ計算Class
        builder.Register<ParameterBuffCalculator>(Lifetime.Singleton);

        //トレーニングイベントの生成を行うクラス
        builder.Register<TrainingEventDataGenerator>(Lifetime.Singleton);

        //育成ゲームのイベント登録用Pool
        builder.Register<TrainingEventPool>(Lifetime.Singleton);

        //一日で行われる育成イベントを決めるクラス
        builder.Register<TrainingEventSelector>(Lifetime.Singleton);
    }
}
