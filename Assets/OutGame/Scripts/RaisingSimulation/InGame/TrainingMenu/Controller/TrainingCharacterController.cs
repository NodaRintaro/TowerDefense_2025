using System;
using UnityEngine;
using VContainer;

/// <summary>
/// TrainingCharacterのデータをゲームのUIに反映させるクラス
/// </summary>
[Serializable]
public class TrainingCharacterController : MonoBehaviour
{
    [SerializeField, Header("トレーニングキャラクター")]
    private TrainingCharacterData _trainingCharacterData = null;

    //ViewClass
    [SerializeField, Header("キャラクターのパラメータView")]
    private CharacterParameterView _parameterView;

    [SerializeField, Header("キャラクターのスタミナのView")]
    private CharacterStaminaView _characterStaminaView;

    //ライフタイムスコープ
    private TrainingCharacterLifeTimeScope _trainingCharacterLifeTimeScope;

    //トレーニングデータ関係
    private TrainingDataHolder _trainingDataManager = null;
    private TrainingSaveDataManager _trainingDataSaveLoadManager = null;
    private ParameterRankSelecter _parameterRankSelecter = null;

    private void Start()
    {
        //ライフタイムスコープのクラスを取得
        _trainingCharacterLifeTimeScope = FindAnyObjectByType<TrainingCharacterLifeTimeScope>();
        ResolveContainerObject();
    }

    /// <summary> コンテナ内のクラスを取得する関数 </summary>
    private void ResolveContainerObject()
    {
        _trainingCharacterData = _trainingCharacterLifeTimeScope.Container.Resolve<TrainingCharacterData>();
        _trainingDataSaveLoadManager = _trainingCharacterLifeTimeScope.Container.Resolve<TrainingSaveDataManager>();
        _trainingDataManager = _trainingCharacterLifeTimeScope.Container.Resolve<TrainingDataHolder>();
        _parameterRankSelecter = _trainingCharacterLifeTimeScope.Container.Resolve<ParameterRankSelecter>();
    }

    public void FirstSetParameter()
    {
        SetParameterGUI(ParameterType.Physical, _trainingCharacterData.TotalPhysical, _trainingCharacterData.ParameterRankDict[ParameterType.Physical]);
        SetParameterGUI(ParameterType.Power, _trainingCharacterData.TotalPower, _trainingCharacterData.ParameterRankDict[ParameterType.Power]);
        SetParameterGUI(ParameterType.Intelligence, _trainingCharacterData.TotalIntelligence, _trainingCharacterData.ParameterRankDict[ParameterType.Intelligence]);
        SetParameterGUI(ParameterType.Speed, _trainingCharacterData.TotalSpeed, _trainingCharacterData.ParameterRankDict[ParameterType.Speed]);
    }

    public void SetParameterGUI(ParameterType parameterType, uint totalParam, RankData rankData)
    {
        _parameterView.SetParameter(parameterType, totalParam, rankData);
    }
}
    
    
