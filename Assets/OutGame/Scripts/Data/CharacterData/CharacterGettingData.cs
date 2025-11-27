using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterGettingData
{
    [SerializeField]
    private bool[] _gettingCharacterData = null;

    public void SetCharacterDataLangth(int length)
    {
        _gettingCharacterData = new bool[length];


    }

    public void OpenCharacter()
    {

    }
}
