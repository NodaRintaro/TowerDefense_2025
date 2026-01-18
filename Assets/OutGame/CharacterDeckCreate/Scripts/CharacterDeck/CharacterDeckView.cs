using System.Collections;
using System.Collections.Generic;
using TowerDefenseDeckData;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDeckView : MonoBehaviour
{
    [SerializeField] Image[] _towerDefenseCharacterCardHolder = new Image[_maxDeckNum];

    private const int _maxDeckNum = CharacterDeckData.DeckLength;

    public void SetImage(int deckIndex, Sprite characterSprite)
    {
        if(deckIndex >= 0 && deckIndex < _maxDeckNum)
        {
            _towerDefenseCharacterCardHolder[deckIndex].sprite = characterSprite;
        }
    }
}
