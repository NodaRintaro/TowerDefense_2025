using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeckData
{
    [SerializeField] private uint[] _trainedCharacterDeck;

    public uint[] trainedCharacterDeck => _trainedCharacterDeck;

    public void SetData(uint[] idDatas)
    {
        _trainedCharacterDeck = idDatas;
    }
}
