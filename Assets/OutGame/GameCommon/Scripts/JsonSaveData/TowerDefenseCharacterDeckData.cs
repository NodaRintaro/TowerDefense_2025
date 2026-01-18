using System;
using UnityEngine;

namespace TowerDefenseDeckData
{
    [Serializable]
    public class CharacterDeckDataRegistry : IJsonSaveData
    {
        [SerializeField] private CharacterDeckData[] _characterDeckHolder = new CharacterDeckData[_deckMaxNum];

        private int _currentDefaultDeckNum = 0;

        private const int _deckMaxNum = 5;

        public int CurrentDefaultDeckNum => _currentDefaultDeckNum;
        public CharacterDeckData[] CharacterDeckHolder => _characterDeckHolder;

        public void SetData(int deckNum, CharacterDeckData data)
        {
            _characterDeckHolder[deckNum] = data;
        }

        public CharacterDeckData GetData(int deckNum)
        {
            return _characterDeckHolder[_deckMaxNum];
        }

        public void ChangeDefaultDeck(int deckNum)
        {
            if(deckNum == _deckMaxNum)
            {
                _currentDefaultDeckNum = 0;
            }
            else if(deckNum < 0)
            {
                _currentDefaultDeckNum = _deckMaxNum - 1;
            }
            else
            {
                _currentDefaultDeckNum = deckNum;
            }
        }
    }

    [Serializable]
    public class CharacterDeckData
    {
        [SerializeField] private TowerDefenseCharacterData[] _trainedCharacterDeck = new TowerDefenseCharacterData[DeckLength];

        public const int DeckLength = 12;

        public TowerDefenseCharacterData[] trainedCharacterDeck => _trainedCharacterDeck;

        public void SetData(int deckNum, TowerDefenseCharacterData data)
        {
            _trainedCharacterDeck[deckNum] = data;
        }
    }
}
