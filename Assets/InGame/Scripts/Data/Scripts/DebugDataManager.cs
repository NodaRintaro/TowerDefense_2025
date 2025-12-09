using System;
using UnityEngine;
using TrainedData;

[CreateAssetMenu(menuName = "InGame/DebugDataManager"),Serializable]
public class DebugDataManager : ScriptableObject
{
    [SerializeField] public TrainedCharacterData[] characterDatas;
    [SerializeField] public EnemyData[] enemyDatas;
}
