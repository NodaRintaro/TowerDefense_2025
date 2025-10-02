using System;
using UnityEngine;

[CreateAssetMenu(menuName = "InGame/CharacterData"), Serializable]
public class UnitData : ScriptableObject
{
    [SerializeField] private TrainedCharacterData _trainedCharacterData;
    public Color color;//一時的なキャラを識別するための色
    public string CharacterName => _trainedCharacterData.BaseCharacter.CharacterName;
    /// <summary>
    /// Physicalの合計値
    /// </summary>
    public float Physical => _trainedCharacterData.Physical + _trainedCharacterData.BaseCharacter.Physical;
    /// <summary>
    /// Intelligenceの合計値
    /// </summary>
    public float Intelligence => _trainedCharacterData.Intelligence + _trainedCharacterData.BaseCharacter.Intelligence;
    /// <summary>
    /// Powerの合計値
    /// </summary>
    public float Power => _trainedCharacterData.Power + _trainedCharacterData.BaseCharacter.Power;
    /// <summary>
    /// Speedの合計値
    /// </summary>
    public float AttackSpeed => _trainedCharacterData.Speed + _trainedCharacterData.BaseCharacter.Speed;
    /// <summary>
    /// キャラクターのタイプ
    /// </summary>
    public float cost;
    public RoleType RoleType => _trainedCharacterData.BaseCharacter.RoleType;
    public float defense;
    public float range;
    public float rePlaceInterval;
}