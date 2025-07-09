using System;
using UnityEngine;

[CreateAssetMenu(menuName = "InGame/CharacterData"), Serializable]
public class CharacterData : ScriptableObject
{
    public Color color;
    public string CharacterName;
    public int Cost;
    public float HP;
    public float MP;
    public float AttackPower;
    public float Defense;
}