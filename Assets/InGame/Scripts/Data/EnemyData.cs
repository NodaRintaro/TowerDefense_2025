using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "InGame/EnemyData", order = 1), Serializable]
public class EnemyData : ScriptableObject
{
    [SerializeField] public string enemyName;
    [SerializeField] public int health;
    [SerializeField] public float speed;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackRate;
    [SerializeField] public float damage;
}
