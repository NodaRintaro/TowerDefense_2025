using System;
using UnityEngine;
[CreateAssetMenu(menuName = "InGame/CharacterDataManager"),Serializable]
public class CharacterDataManager : ScriptableObject
{
    [SerializeField] private CharacterData[] characterDatas;
    public CharacterData[] CharacterDatas { get { return characterDatas; } }
}
