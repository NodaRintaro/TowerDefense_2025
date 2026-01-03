using UnityEngine;
using VContainer;

/// <summary>
/// SupportCardが獲得したキャラクターのデータ
/// </summary>
public class SupportCardCollectionData : PlayerCollectionDataBase, IJsonSaveData
{
    public override void AddCollection(uint id)
    {
        if (!CollectionList.Contains(id))
        {
            CollectionList.Add(id);
        }
    }

    public override bool TryGetCollection(uint id)
    {
        if (CollectionList.Contains(id))
        {
            return true;
        }
        return false;
    }
}
