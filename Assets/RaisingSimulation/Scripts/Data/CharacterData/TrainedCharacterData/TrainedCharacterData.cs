using UnityEngine;

/// <summary> トレーニング済みのキャラクターデータ </summary>
[System.Serializable]
public class TrainedCharacterData
{
    [SerializeField, Header("トレーニング後のキャラクターID")]
    private string _id;

    [SerializeField, Header("ベースとなるキャラクターのデータ")]
    private CharacterData _baseCharacter;

    [SerializeField, Header("体力増加値")]
    private uint _physical;
    [SerializeField, Header("筋力増加値")]
    private uint _power;
    [SerializeField, Header("知力増加値")]
    private uint _intelligence;
    [SerializeField, Header("素早さ増加値")]
    private uint _speed;

    /// <summary> トレーニング後のキャラクターデータID </summary>
    public string TrainedCharacterID => _id;

    #region 増加値の参照用プロパティ
    public CharacterData BaseCharacter => _baseCharacter;
    public uint Physical => _physical;
    public uint Power => _power;
    public uint Intelligence => _intelligence;
    public uint Speed => _speed;
    #endregion

    #region 合計値の参照プロパティ
    public uint TotalPhysical => _physical + _baseCharacter.Physical;
    public uint TotalPower => _power + _baseCharacter.Power;
    public uint TotalIntelligence => _intelligence + _baseCharacter.Intelligence;
    public uint TotalSpeed => _speed + _baseCharacter.Speed;
    #endregion

    public void SetCharacterTrainedData(CharacterData setChara, string newID, uint setPhysi, uint setPow, uint setInt, uint setSp)
    {
        _id = newID;
        _baseCharacter = setChara;
        _physical = setPhysi;
        _power = setPow;
        _intelligence = setInt;
        _speed = setSp;
    }
}
