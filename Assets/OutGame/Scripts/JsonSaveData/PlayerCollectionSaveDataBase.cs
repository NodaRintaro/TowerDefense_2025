using JetBrains.Annotations;
using System.Collections.Generic;

/// <summary>
/// プレイヤーの収集したデータを保存するクラスの基底クラス
/// </summary>
public abstract class PlayerCollectionSaveDataBase : JsonSaveDataBase
{
    protected Dictionary<uint, uint> _collectionData = new();

    public Dictionary<uint, uint> CollectionData => _collectionData;

    public abstract void AddCollection(uint id);
    
    public abstract uint GetCollection(uint id);
}