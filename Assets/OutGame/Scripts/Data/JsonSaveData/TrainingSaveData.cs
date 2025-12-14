using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;
using CharacterData;

/// <summary>
/// TrainingCharcterに関するデータを保持するClass
/// </summary>
public class TrainingSaveData : SaveDataBase
{
    private TrainingCharacterData _characterData = null;

    private TrainingSupportCardDeckData _cardDeckData = new();

    public TrainingCharacterData TrainingCharacterData => _characterData;

    public TrainingSupportCardDeckData TrainingCardDeckData => _cardDeckData;

    /// <summary> キャラクターのデータをセット </summary>
    public void SetCharacterData(CharacterBaseData characterData)
    {
        if(_characterData == null)
        {
            _characterData = new TrainingCharacterData(characterData);
        }
        else
        {
            _characterData.SetBaseData(characterData);
        }
    }

    public void SetCardData(uint deckNum, SupportCardData cardData)
    {
        _cardDeckData.CardPutInDeck(deckNum, cardData);
    }

    public class TrainingSupportCardDeckData
    {
        private SupportCardData[] _cardDeckData = new SupportCardData[_deckNum];

        private const int _deckNum = 4;

        public SupportCardData CardDeckData => _cardDeckData[_deckNum];

        public void CardPutInDeck(uint deckNum, SupportCardData cardData)
        {
            if(deckNum >= _deckNum)
            {
                _cardDeckData[deckNum] = cardData;
            }
            else
            {
                Debug.Log("デッキに当てはまる要素番号が見つかりませんでした");
            }
        }
    }
}