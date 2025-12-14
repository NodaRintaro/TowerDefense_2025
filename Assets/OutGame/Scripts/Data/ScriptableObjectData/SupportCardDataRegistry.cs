using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SupportCardDataHolder", menuName = "ScriptableObject/SupportCardDataHolder")]
public class SupportCardDataRegistry : DataRegistryBase<SupportCardData>
{
    public SupportCardData GetData(uint id)
    {
        foreach(var data in _dataHolder)
        {
            if(data.ID == id)
            {
                return data;
            }
        }
        return null;
    }
}
