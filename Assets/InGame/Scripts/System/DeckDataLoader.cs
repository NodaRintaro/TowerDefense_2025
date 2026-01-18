using TowerDefenseDeckData;

public class DeckDataLoader
{
    //タワーディフェンスで使うデッキのデータ
    private static CharacterDeckData _currentUseDeck = null;
    
    public static CharacterDeckData CurrentUseDeck => _currentUseDeck;

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
