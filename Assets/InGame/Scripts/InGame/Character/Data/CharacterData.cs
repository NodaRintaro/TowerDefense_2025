using System;
using UnityEngine;

[CreateAssetMenu(menuName = "InGame/CharacterData"), Serializable]
public class CharacterData : ScriptableObject
{
    public Color Color;
    public CharacterType CharacterType;
    public string CharacterName;
    public float Cost;
    public float HP;
    public float MP;
    public float AttackPower;
    public float Defense;
    public float AttackSpeed;
    public float Range;
}

public enum ChatacterType
{
    Attacker,
    Defender,
    Healer,
}