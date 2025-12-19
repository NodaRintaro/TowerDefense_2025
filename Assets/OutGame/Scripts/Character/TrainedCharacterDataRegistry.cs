using CharacterData;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "TrainedCharacterRegistry", menuName = "ScriptableObject/TrainedCharacterRegistry")]
public class TrainedCharacterDataRegistry : MasterDataBase<TrainedCharacterData>
{
    public TrainedCharacterData GetData(uint id)
    {
        foreach (var data in _dataHolder)
        {
            if (data.TrainedCharacterID == id)
            {
                return data;
            }
        }
        return null;
    }
}