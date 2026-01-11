using NovelData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainingEventData", menuName = "ScriptableObject/TrainingEventData")]
public class TrainingEventDataRegistry : DataRegistryBase<TrainingEventData>
{
    public TrainingEventData GetData(uint id)
    {
        foreach (var item in _dataHolder)
        {
            if (item.TrainingEventID == id)
            {
                return item;
            }
        }
        return null;
    }
}
