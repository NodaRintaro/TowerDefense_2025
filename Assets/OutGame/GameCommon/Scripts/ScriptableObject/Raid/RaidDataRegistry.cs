using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RaidDataRegistry", menuName = "ScriptableObject/RaidDataRegistry")]
public class RaidDataRegistry : DataRegistryBase<RaidData>
{
    public RaidData GetData(uint id)
    {
        foreach (var item in _dataHolder)
        {
            if (item.RaidID == id)
            {
                return item;
            }
        }
        return null;
    }
}
