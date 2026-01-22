using JetBrains.Annotations;
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// プレイヤーの収集したデータを保存するクラスの基底クラス
/// </summary>
public abstract class PlayerCollectionDataBase
{
    /// <summary> コレクションのデータ </summary>
    [JsonProperty] protected HashSet<uint> _collectionDataList = new();

    public HashSet<uint> CollectionList => _collectionDataList;

    public abstract void AddCollection(uint id);
    
    public abstract bool TryGetCollection(uint id);
}