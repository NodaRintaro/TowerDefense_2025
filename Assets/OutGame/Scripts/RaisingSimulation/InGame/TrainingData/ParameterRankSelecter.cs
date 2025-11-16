using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using UniRx;
 
public class ParameterRankSelecter
{
    private TrainingCharacterData _trainingCharacterData;
    private RankDataHolder _rankDataHolder;

    [Inject]
    public ParameterRankSelecter(TrainingDataHolder trainingDataManager, RankDataHolder rankDataHolder)
    {
        _trainingCharacterData = trainingDataManager.TrainingCharacterData;
        _rankDataHolder = rankDataHolder;
        RankSubscribe();
    }

    public void RankSubscribe()
    {
        _trainingCharacterData.CurrentPhysicalBuff.Subscribe(value => RankSelect(ParameterType.Physical, _trainingCharacterData.TotalPhysical));
        _trainingCharacterData.CurrentPowerBuff.Subscribe(value => RankSelect(ParameterType.Power, _trainingCharacterData.TotalPower));
        _trainingCharacterData.CurrentIntelligenceBuff.Subscribe(value => RankSelect(ParameterType.Intelligence, _trainingCharacterData.TotalIntelligence));
        _trainingCharacterData.CurrentSpeedBuff.Subscribe(value => RankSelect(ParameterType.Speed, _trainingCharacterData.TotalSpeed));
    }

    public void RankSelect(ParameterType parameterType, uint param)
    {
        foreach (var rank in _rankDataHolder.RankList)
        {
            if(rank.RankUpNum > param)
            {
                _trainingCharacterData.ParameterRankDict[parameterType] = rank;
            }
        }
    }
}
