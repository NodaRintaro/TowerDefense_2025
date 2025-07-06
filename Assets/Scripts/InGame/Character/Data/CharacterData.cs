
using System;
using UnityEngine;
[CreateAssetMenu(menuName = "InGame/CharacterData"),Serializable]
public class CharacterData : ScriptableObject
{
    [SerializeField] private Color color;
    [SerializeField] private string CharacterName;
    [SerializeField]  private float HP;
    [SerializeField] private float MP;
    [SerializeField] private float AttackPower;
    [SerializeField] private float Defense;
}
