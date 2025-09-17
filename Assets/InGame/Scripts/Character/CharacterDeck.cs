
public class CharacterDeck
{
    private UnitData[] _characterDatas;
    private bool[] _canPlaceCharacter;
    private float[] _intervalTimer;
    public int Count { get { return _characterDatas.Length; } }

    void UpdateTime(float time)
    {
        for (int i = 0; i < _characterDatas.Length; i++)
        {
            if (_intervalTimer[i] > 0)
            {
                _intervalTimer[i] -= time;
                if (_intervalTimer[i] <= 0)
                {
                    _canPlaceCharacter[i] = true;
                }
            }
        }
    }
    public CharacterDeck(UnitData[] characterDatas)
    {
        _characterDatas = characterDatas;
        _canPlaceCharacter = new bool[characterDatas.Length];
        _intervalTimer = new float[characterDatas.Length];
        for(int i = 0; i < _canPlaceCharacter.Length; i++){_canPlaceCharacter[i] = true;}
    }
    public UnitData GetCharacterData(int index)
    {
        return _characterDatas[index];
    }
    /// <summary>
    /// Trueなら配置可能
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool CanPlaceCharacter(int index)
    {
        return _canPlaceCharacter[index];
    }

    public void SetIntervalTimer(int index, float interval)
    {
        _intervalTimer[index] = interval;
    }

    public void SetCanPlaceCharacter(int index, bool canPlace)
    {
        _canPlaceCharacter[index] = canPlace;
    }
}
