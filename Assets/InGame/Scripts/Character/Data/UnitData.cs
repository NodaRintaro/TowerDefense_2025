using System;
using UnityEngine;

[CreateAssetMenu(menuName = "InGame/CharacterData"), Serializable]
public class UnitData : ScriptableObject
{
    public Color Color;
    public string CharacterName;
    public float Cost;
    public float HP;
    public float MP;
    public float AttackPower;
    public float Defense;
    public float AttackSpeed;
    public float Range;
}