using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingSupportCardDeckData
{
    private SupportCardData[] _cardDeckData = new SupportCardData[_deckNum];

    private const int _deckNum = 4;

    public SupportCardData[] CardDeckData => _cardDeckData;

    public void CardPutInDeck(uint deckNum, SupportCardData cardData)
    {
        if (deckNum >= _deckNum)
        {
            _cardDeckData[deckNum] = cardData;
        }
        else
        {
            Debug.Log("デッキに当てはまる要素番号が見つかりませんでした");
        }
    }
}
