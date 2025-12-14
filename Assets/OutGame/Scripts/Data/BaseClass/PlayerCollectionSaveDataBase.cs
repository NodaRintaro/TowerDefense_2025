using JetBrains.Annotations;
using System.Collections.Generic;

/// <summary>
/// プレイヤーの収集したデータを保存するクラスの基底クラス
/// </summary>
public abstract class PlayerCollectionSaveDataBase : SaveDataBase
{
    protected static Dictionary<uint, uint> _collectionData = null;

    protected static Dictionary<uint, uint> CollectionData => _collectionData;

    public abstract void AddCollection(uint id);
    
    public abstract uint GetCollection(uint id);
}