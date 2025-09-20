using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Wave Data", menuName = "InGame/Wave Data")]
public class WaveData : ScriptableObject
{
       public EnemyGenerateData[] enemyGenerateData;
       public int Count => enemyGenerateData.Length;

       //渡された時間が敵の生成時間を超えたかどうかを判定
       public bool IsOverGenerateTime(float time, int waveIndex)
       {
              if (enemyGenerateData[waveIndex].spawnTime > time) return false;
              return true;
       }
}
[Serializable]
public class EnemyGenerateData
{
       public EnemyData enemyData;
       public float spawnTime;
}
