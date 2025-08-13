using UnityEngine;

/// <summary> トレーニング済みのキャラクターデータ </summary>
[System.Serializable]
public class TrainedCharacterData
{
    [SerializeField, Header("強化したキャラクターのデータ")]
    private CharacterData _baseCharacter;
    [SerializeField, Header("体力増加値")]
    private uint _physical;
    [SerializeField, Header("筋力増加値")]
    private uint _power;
    [SerializeField, Header("知力増加値")]
    private uint _intelligence;
    [SerializeField, Header("素早さ増加値")]
    private uint _speed;

    #region 参照用プロパティ
    public CharacterData BaseCharacter => _baseCharacter;
    public uint Physical => _physical;
    public uint Power => _power;
    public uint Intelligence => _intelligence;
    public uint Speed => _speed;
    #endregion

    public void SetCharacterTrainedData(CharacterData setChara, uint setPhysi, uint setPow, uint setInt, uint setSp)
    {
        _baseCharacter = setChara;
        _physical = setPhysi;
        _power = setPow;
        _intelligence = setInt;
        _speed = setSp;
    }
}
