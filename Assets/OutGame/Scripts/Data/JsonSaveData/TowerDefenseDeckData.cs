using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefenseDeckData
{
    [Serializable]
    public class CharacterDeckSaveData : SaveDataBase
    {
        [SerializeField] private CharacterDeckData[] _characterDeckHolder = new CharacterDeckData[_deckNum];

        private const int _deckNum = 6;

        public CharacterDeckData[] CharacterDeckHolder => _characterDeckHolder;

        public void SetData(int deckNum, CharacterDeckData data)
        {
            _characterDeckHolder[deckNum] = data;
        }

        public CharacterDeckData GetData(int deckNum)
        {
            return _characterDeckHolder[_deckNum];
        }
    }

    [Serializable]
    public class CharacterDeckData
    {
        [SerializeField] private uint[] _trainedCharacterDeck = new uint[_deckLength];

        private const int _deckLength = 12;

        public uint[] trainedCharacterDeck => _trainedCharacterDeck;

        public void SetData(int deckNum, uint id)
        {
            _trainedCharacterDeck[deckNum] = id;
        }
    }
}
