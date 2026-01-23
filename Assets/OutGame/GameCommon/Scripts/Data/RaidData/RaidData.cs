using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RaidData
{
    [SerializeField] private uint _raidID;
    [SerializeField] private uint _raidMapID;
    [SerializeField] private List<uint> _characterIDs = new List<uint>();
    
    public uint RaidID => _raidID;
    public List<uint> CharacterIDs => _characterIDs;
    
#if UNITY_EDITOR
    public void SetCharacterTeam(List<uint> characterIDs)
    {
        _characterIDs = characterIDs;
    }

    public void SetID(uint id)
    {
        _raidID = id;
    }
#endif
}
