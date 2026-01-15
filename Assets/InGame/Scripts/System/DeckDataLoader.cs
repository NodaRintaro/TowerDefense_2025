using System.Collections;
using System.Collections.Generic;
using TowerDefenseDeckData;
using UnityEngine;

public class DeckDataLoader
{
    //タワーディフェンスで使うデッキのデータ
    private static CharacterDeckData _currentUseDeck = null;

    /// <summary> デッキのデータをセットする </summary>
    public static void SetDeck(CharacterDeckData characterDeckData)
    {
        _currentUseDeck = characterDeckData;
    }

    /// <summary> デッキのデータを取得する </summary>
    public static CharacterDeckData GetDeck()
    {
        return _currentUseDeck;
    }
}
