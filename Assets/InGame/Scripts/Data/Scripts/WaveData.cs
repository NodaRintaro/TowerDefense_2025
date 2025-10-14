using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New Wave Data", menuName = "InGame/Wave Data"), Serializable]
public class WaveData : ScriptableObject
{
       public AIRoute aiRoute;
       public EnemyGenerateData[] enemyGenerateDatas;
       public int Count => enemyGenerateDatas.Length;

       //渡された時間が敵の生成時間を超えたかどうかを判定
       public bool IsOverGenerateTime(float time, int waveIndex)
       {
              if (enemyGenerateDatas[waveIndex].spawnTime > time) return false;
              return true;
       }

       public WaveData()
       {
              aiRoute = new AIRoute();
              enemyGenerateDatas = new EnemyGenerateData[0];
       }
}
[Serializable]
public class EnemyGenerateData
{
       public EnemyData enemyData;
       public float spawnTime;
}
