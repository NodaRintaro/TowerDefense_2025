using System;

[Serializable]
public class WaveData
{
    public AIRoute aiRoute;
    public EnemyGenerateData[] enemyGenerateDatas;
    public int Count => enemyGenerateDatas.Length;
    private int _index = 0;

    //渡された時間が敵の生成時間を超えたかどうかを判定
    public bool IsOverGenerateTime(float time)
    {
        if (enemyGenerateDatas[_index].spawnTime > time) return false;
        return true;
    }

    /// <summary>
    /// 敵のデータを取得する
    /// </summary>
    /// <returns></returns>
    public EnemyData GetEnemyData()
    {
        _index++;
        return enemyGenerateDatas[_index - 1].enemyData;
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