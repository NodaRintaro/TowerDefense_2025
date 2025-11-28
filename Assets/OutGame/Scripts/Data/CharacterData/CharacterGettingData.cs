using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterGettingData
{
    private Dictionary<uint, bool> _characterGettingDict = new();

    public Dictionary<uint, bool> CharacterGettingDict => _characterGettingDict;

    public bool IsGettingCharacter(uint characterId)
    {
        return _characterGettingDict[characterId];
    }

    public void CharacetrGetting(uint characterId)
    {
        _characterGettingDict[characterId] = true;
    }
}
