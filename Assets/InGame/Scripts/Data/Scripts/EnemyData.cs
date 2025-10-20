using System;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "Tools/EnemyData", order = 1), Serializable]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int hp;
    public float moveSpeed;
    public float range;
    public float attackRate;
    public float attack;
    public float defence;
}
