using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BranchTrainingEventData", menuName = "ScriptableObject/BranchTrainingEventData")]
public class BranchTrainingEventDataRegistry : DataRegistryBase<BranchTrainingEventData>
{
    public BranchTrainingEventData GetData(int id)
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

    public List<BranchTrainingEventData> GetBranchEvents(uint id)
    {
        List<BranchTrainingEventData> eventList = new List<BranchTrainingEventData>();
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
