using System;
using UnityEngine;

[CreateAssetMenu(menuName = "InGame/UnitDataManager"),Serializable]
public class UnitDataManager : ScriptableObject
{
    [SerializeField] private UnitData[] characterDatas;
    public UnitData[] CharacterDatas { get { return characterDatas; } }
    public UnitData GetCharacterData(int index)
    {
        return characterDatas[index];
    }
}
