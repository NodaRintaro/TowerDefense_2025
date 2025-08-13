using UnityEngine;

/// <summary> �g���[�j���O�ς݂̃L�����N�^�[�f�[�^ </summary>
[System.Serializable]
public class TrainedCharacterData
{
    [SerializeField, Header("���������L�����N�^�[�̃f�[�^")]
    private CharacterData _baseCharacter;
    [SerializeField, Header("�̗͑����l")]
    private uint _physical;
    [SerializeField, Header("�ؗ͑����l")]
    private uint _power;
    [SerializeField, Header("�m�͑����l")]
    private uint _intelligence;
    [SerializeField, Header("�f���������l")]
    private uint _speed;

    #region �Q�Ɨp�v���p�e�B
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
