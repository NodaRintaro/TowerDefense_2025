using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// キャラクターごとのイベントを保管するScriptableObject
/// </summary>
public class CharacterEventDataRegistry : DataRegistryBase<CharacterEventDataBase>
{
    public CharacterEventDataBase GetData(int id)
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
/// キャラクター1種類分のイベントデータベース
/// </summary>
[Serializable]
public class CharacterEventDataBase
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

    #if UNITY_EDITOR
    public void SetCharacterID(uint id) => _characterID = id;

    public void SetEventData(TrainingEventData[] trainingEvent) => _trainingEventDataHolder = trainingEvent;
    #endif
}