using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターごとの分岐イベントを保管するScriptableObject
/// </summary>
public class CharacterBranchEventDataRegistry : DataRegistryBase<BranchCharacterEventDataBase> 
{
    public BranchCharacterEventDataBase GetData(uint id)
    {
        foreach (var item in _dataHolder)
        {
            if (item.CharacterID == id)
            {
                return item;
            }
        }
        return null;
    }
}

/// <summary>
/// キャラクター1種類分の分岐イベントデータベース
/// </summary>
[Serializable]
public class BranchCharacterEventDataBase
{
    [SerializeField] private uint _characterID;

    [SerializeField] TrainingEventData[] _trainingEventDataHolder = null;

    public uint CharacterID => _characterID;

    public TrainingEventData GetEvent(int id)
    {
        foreach (var item in _trainingEventDataHolder)
        {
            if (item.EventID == id)
            {
                return item;
            }
        }
        return null;
    }

    public List<TrainingEventData> GetBranchEvents(uint id)
    {
        List<TrainingEventData> eventList = new List<TrainingEventData>();
        foreach (var item in _trainingEventDataHolder)
        {
            if (item.EventID == id)
            {
                eventList.Add(item);
            }
        }
        return eventList;
    }

#if UNITY_EDITOR
    public void SetCharacterID(uint id) => _characterID = id;

    public void SetEventData(TrainingEventData[] trainingEvent) => _trainingEventDataHolder = trainingEvent;
#endif
}
