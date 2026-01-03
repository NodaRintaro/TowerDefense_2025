using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// Playerが獲得したキャラクターのデータ
/// </summary>
public class CharacterCollectionData : PlayerCollectionDataBase, IJsonSaveData
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
