using System;

namespace TD.Game
{
    /// <summary>1体スポーンの定義（例：Wave内の要素）</summary>
    [Serializable]
    public sealed class EnemySpawnDefinition
    {
        public string enemyId;   // どの敵プレハブか
        public string routeId;   // どのルートを通るか
        public float spawnTime;  // Wave開始からの秒
        public int spawnIndex = -1; // -1 = random
        public int goalIndex  = -1; // -1 = random
    }
}