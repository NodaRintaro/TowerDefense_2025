using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BranchTrainingEventData", menuName = "ScriptableObject/BranchTrainingEventData")]
public class TrainingBranchEventDataRegistry : DataRegistryBase<TrainingEventData>
{
    public TrainingEventData GetData(int id)
    {
        foreach (var item in _dataHolder)
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
        foreach (var item in _dataHolder)
        {
            if (item.EventID == id)
            {
                eventList.Add(item);
            }
        }
        return eventList;
    }

}
