using System;
using UnityEngine;
using VContainer;
using TrainingData;
using RankData;

/// <summary>
/// TrainingCharacterのデータをゲームのUIに反映させるクラス
/// </summary>
[Serializable]
public class TrainingCharacterController : MonoBehaviour
{
    [SerializeField, Header("トレーニングキャラクター")]
    private TrainingCharacterData _trainingCharacterData = null;

    //ViewClass
    //[SerializeField, Header("キャラクターのパラメータView")]
    //private CharacterParameterUI _parameterView;

    [SerializeField, Header("キャラクターのスタミナのView")]
    private CharacterStaminaView _characterStaminaView;

    //ライフタイムスコープ
    private RaisingSimulationInGameLifeTimeScope _trainingCharacterLifeTimeScope;

    //トレーニングデータ関係
    private TrainingDataHolder _trainingDataManager = null;

    private void Start()
    {
        //ライフタイムスコープのクラスを取得
        _trainingCharacterLifeTimeScope = FindAnyObjectByType<RaisingSimulationInGameLifeTimeScope>();
        ResolveContainerObject();
    }

    /// <summary> コンテナ内のクラスを取得する関数 </summary>
    private void ResolveContainerObject()
    {
        _trainingCharacterData = _trainingCharacterLifeTimeScope.Container.Resolve<TrainingCharacterData>();
        _trainingDataManager = _trainingCharacterLifeTimeScope.Container.Resolve<TrainingDataHolder>();
    }

    public void FirstSetParameter()
    {
        SetParameterGUI(ParameterType.Physical, _trainingCharacterData.TotalPhysical, _trainingCharacterData.ParameterRankDict[ParameterType.Physical]);
        SetParameterGUI(ParameterType.Power, _trainingCharacterData.TotalPower, _trainingCharacterData.ParameterRankDict[ParameterType.Power]);
        SetParameterGUI(ParameterType.Intelligence, _trainingCharacterData.TotalIntelligence, _trainingCharacterData.ParameterRankDict[ParameterType.Intelligence]);
        SetParameterGUI(ParameterType.Speed, _trainingCharacterData.TotalSpeed, _trainingCharacterData.ParameterRankDict[ParameterType.Speed]);
    }

    public void SetParameterGUI(ParameterType parameterType, uint totalParam, RankSpriteData rankData)
    {
        //_parameterView.SetParameter(parameterType, totalParam, rankData);
    }
}
    
    
