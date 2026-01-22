using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterTrainingEventMapRegistry", menuName = "ScriptableObject/CharacterTrainingEventMapRegistry")]
public class CharacterTrainingScheduleRegistry : DataRegistryBase<CharacterTrainingSchedule>
{
    public CharacterTrainingSchedule GetData(uint id)
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
