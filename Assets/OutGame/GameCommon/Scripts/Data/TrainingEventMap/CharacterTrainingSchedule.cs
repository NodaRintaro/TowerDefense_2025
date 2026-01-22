using System;
using UnityEngine;

/// <summary>
/// キャラクターごとに設定された育成期間とその内容のデータ
/// </summary>
[Serializable]
public class CharacterTrainingSchedule
{
    [SerializeField] private uint _characterID;

    [SerializeField] OneDayEvents[] _trainingEventSchedule;

    public uint CharacterID => _characterID;
    public OneDayEvents[] TrainingEventSchedule => _trainingEventSchedule;

    #if UNITY_EDITOR
    public void SetCharacterTrainingSchedule(OneDayEvents[] oneDayEvents)
    {
        _trainingEventSchedule = oneDayEvents;
    }

    public void SetID(uint id)
    {
        _characterID = id;
    }
    #endif
}

/// <summary>
/// 一日で行われる固有イベントEvent
/// </summary>
[Serializable]
public struct OneDayEvents
{
    public bool IsRaid;

    public uint FirstUniqueEvent;

    public uint SecondUniqueEvent;

    public uint LastUniqueEvent;
}
