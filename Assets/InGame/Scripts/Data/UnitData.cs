using System;
using UnityEngine;

[CreateAssetMenu(menuName = "InGame/CharacterData"), Serializable]
public class UnitData : ScriptableObject
{
    public Color color;
    public string characterName;
    public float cost;
    public float hp;
    public float mp;
    public float attackPower;
    public float defense;
    public float attackSpeed;
    public float range;
    public float rePlaceInterval;
}