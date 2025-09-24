using System;
using UnityEngine;

[CreateAssetMenu(menuName = "InGame/DebugDataManager"),Serializable]
public class DebugDataManager : ScriptableObject
{
    [SerializeField] public UnitData[] characterDatas;
    [SerializeField] public EnemyData[] enemyDatas;
}
