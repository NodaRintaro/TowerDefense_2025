using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterHaveData : MonoBehaviour
{
    private uint _characterId;
    private int _index;
    public uint CharacterId => _characterId;
    public int Index=> _index;
    
    public void SetCharacterId(uint id)
    {
        _characterId = id;
    }
    
    public void SetIndex(int index)
    {
        _index = index;
    }
}
