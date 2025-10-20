using System;
using UnityEngine;

[Serializable]
public class WaveData
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
    public EnemyGenerateData(EnemyData enemyData, float spawnTime)
    {
        this.enemyData = enemyData;
        this.spawnTime = spawnTime;
    }
}