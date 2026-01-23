using UnityEngine;
public class UnitDeck
{
    private PlayerUnitData[] _unitDatas;
    private bool[] _canPlaceCharacter;
    public PlayerUnitData[] UnitDatas => _unitDatas;
    public UnitDeck(TowerDefenseCharacterData[] trainedCharacterDatas)
    {
        _unitDatas = new PlayerUnitData[trainedCharacterDatas.Length];
        for (int i = 0; i < trainedCharacterDatas.Length; i++)
        {
            _unitDatas[i] = new PlayerUnitData(trainedCharacterDatas[i]);
        }
        _canPlaceCharacter = new bool[trainedCharacterDatas.Length];
        for(int i = 0; i < _canPlaceCharacter.Length; i++){_canPlaceCharacter[i] = true;}
    }
    public void UpdateTime(float time)
    {
        for (int i = 0; i < _unitDatas.Length; i++)
        {
            if (_unitDatas[i].RePlaceTimer > 0)
            {
                _unitDatas[i].RePlaceTimer -= time;
                if (_unitDatas[i].RePlaceTimer <= 0)
                {
                    _canPlaceCharacter[i-1] = true;
                    Debug.Log($"index:{i} 配置可能になりました");
                }
            }
        }
    }
    public PlayerUnitData GetCharacterData(int index)
    {
        return _unitDatas[index];
    }
    /// <summary>
    /// Trueなら配置可能
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool CanPlaceCharacter(int index)
    {
        Debug.Log($"index:{index} canPlace:{_canPlaceCharacter[index]}");
        return _canPlaceCharacter[index];
    }
    /// <summary>
    /// 再配置時間を設定
    /// </summary>
    private void SetRePlaceTimer(uint index, float interval)
    {
        _unitDatas[index].RePlaceTimer = interval;
    }
    
    public void CharacterRemoved(uint index)
    {
        SetRePlaceTimer(index, _unitDatas[index].RePlaceInterval);
        Debug.Log("要修正");
    }

    /// <summary>
    /// キャラクターの配置可能状態を設定
    /// </summary>
    /// <param name="index">チームのインデックス</param>
    /// <param name="canPlace"></param>
    public void SetCanPlaceCharacter(int index, bool canPlace)
    {
        _canPlaceCharacter[index] = canPlace;
    }
}
