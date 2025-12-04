using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeckRegistry
{
    [SerializeField] private uint[] _characterDeckRegistry; 

    public uint[] characterDeckRegistry => _characterDeckRegistry;

    public void SetData(uint[] characterDeckRegistry)
    {
        _characterDeckRegistry = characterDeckRegistry;
    }
}
