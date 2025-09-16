
public class CharacterDeck
{
    private UnitData[] _characterDatas;
    private bool[] _canPlaceCharacter;
    public int Count { get { return _characterDatas.Length; } }

    public CharacterDeck(UnitData[] characterDatas)
    {
        _characterDatas = characterDatas;
        _canPlaceCharacter = new bool[characterDatas.Length];
    }
    public UnitData GetCharacterData(int index)
    {
        return _characterDatas[index];
    }

    public bool CanPlaceCharacter(int index)
    {
        return _canPlaceCharacter[index];
    }
}
