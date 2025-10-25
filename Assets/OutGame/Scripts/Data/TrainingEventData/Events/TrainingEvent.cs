﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingEvent: IEventData
{
    [SerializeField,Header("トレーニングの種類")] 
    private TrainingType _trainingType;

    [SerializeField,Header("トレーニングによって発生するコスト")]
    private int _trainingCost = 0;

    [SerializeField, Header("トレーニングによって上昇するステータス一覧")] 
    private List<EventStatusBuff> _trainingBuffList;

    /// <summary> キャラクター強化の追加強化倍率 </summary>
    private uint _addBuffPercentNum;

    public TrainingType TrainingType => _trainingType;

    public void SetAddBuffPercentNum(uint bonusNum) => _addBuffPercentNum = bonusNum;

    public void OnTrainingEvent(TrainingCharacterData trainingCharacterData)
    {
        StatusBuff(trainingCharacterData);
    }

    private void StatusBuff(TrainingCharacterData trainingCharacterData)
    {
        foreach (var training in _trainingBuffList)
        {
            training.BuffStatus(trainingCharacterData, _addBuffPercentNum);
        }
    }
}

