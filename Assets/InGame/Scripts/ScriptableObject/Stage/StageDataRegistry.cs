using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataRegistry", menuName = "ScriptableObject/StageDataRegistry")]
public class StageDataRegistry : DataRegistryBase<StageData>
{
    public StageData GetData(uint id)
    {
        foreach (var item in _dataHolder)
        {
            if (item.stageID == id)
            {
                return item;
            }
        }
        return null;
    }
}
