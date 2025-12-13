using UnityEngine;

namespace TowerDefenseDeckData
{
    public class CharacterDeckRegistry
    {
        [SerializeField] private uint[] _characterDeckRegistry;

        public uint[] characterDeckRegistry => _characterDeckRegistry;

        public void SetData(uint[] characterDeckRegistry)
        {
            _characterDeckRegistry = characterDeckRegistry;
        }
    }

    public class CharacterDeckData
    {
        [SerializeField] private uint[] _trainedCharacterDeck;

        public uint[] trainedCharacterDeck => _trainedCharacterDeck;

        public void SetData(uint[] idDatas)
        {
            _trainedCharacterDeck = idDatas;
        }
    }

}
