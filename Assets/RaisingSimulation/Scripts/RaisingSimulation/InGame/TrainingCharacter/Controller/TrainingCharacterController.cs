using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// TrainingCharacterのデータをゲームのUIに反映させるクラス
/// </summary>
[Serializable]
public class TrainingCharacterController : MonoBehaviour
{
    //ViewClass
    [SerializeField, Header("キャラクターのパラメータView")] 
    private CharacterParameterView _parameterView;
    [SerializeField, Header("キャラクターのスタミナのView")] 
    private CharacterStaminaView _characterStaminaView;

    //トレーニングのデータ
    [SerializeField, Header("トレーニング中のキャラクターなどのデータ")] 
    private TrainingDataHolder _trainingData;

    private TrainingInGameLifeTimeScope _trainingInGameLifeTimeScope;

    private TrainingDataManager _trainingDataManager;

    private void Start()
    {
        _trainingInGameLifeTimeScope = FindAnyObjectByType<TrainingInGameLifeTimeScope>();
        GetContainerObject();

        UpdateTrainingData();
    }


    /// <summary> コンテナ内のクラスを取得する関数 </summary>
    private void GetContainerObject()
    {
        _trainingData = _trainingInGameLifeTimeScope.Container.Resolve<TrainingDataHolder>();
        _trainingDataManager = _trainingInGameLifeTimeScope.Container.Resolve<TrainingDataManager>();
    }

    /// <summary> 最新のトレーニングデータをUIに表示させる処理 </summary>
    public void UpdateTrainingData()
    {
        
    }
}
