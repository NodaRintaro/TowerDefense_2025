using System;

[Serializable]
public class WaveData
{
    public AIRoute aiRoute;
    public EnemyGenerateData[] enemyGenerateDatas;
    public int EnemyNumsInWave => enemyGenerateDatas.Length;
    /// <summary>
    /// 渡された時間が敵の生成時間を超えたかどうかを判定
    /// </summary>
    /// <param name="time">インゲーム内の経過時間</param>
    /// <returns></returns>
    public bool IsOverGenerateTime(float time, int index)
    {
        if (index >= enemyGenerateDatas.Length || enemyGenerateDatas[index].spawnTime > time) return false;
        return true;
    }

    /// <summary>
    /// 敵のデータを取得する
    /// </summary>
    /// <returns></returns>
    public EnemyUnitData GetEnemyUnitData(int index)
    {
        EnemyUnitData enemyUnitData = new EnemyUnitData(enemyGenerateDatas[index].enemyData);
        //enemyUnitData.AiRoute = aiRoute;
        return enemyUnitData;
    }

    public WaveData()
    {
        aiRoute = new AIRoute();
        enemyGenerateDatas = new EnemyGenerateData[0];
    }

    public bool IsWaveEnd(int index)
    {
        return index >= enemyGenerateDatas.Length;
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