using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "InGame/EnemyData", order = 1), Serializable]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int hp;
    public float speed;
    public float range;
    public float attackRate;
    public float attack;
    public float defence;
}
