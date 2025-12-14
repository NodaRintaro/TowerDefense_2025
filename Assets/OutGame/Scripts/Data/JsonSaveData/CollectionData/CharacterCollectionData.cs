using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollectionData : PlayerCollectionSaveDataBase
{
    public override void AddCollection(uint id)
    {
        const int addValue = 1;
        const int maxValue = 3;
        if (!_collectionData.ContainsKey(id))
        {
            _collectionData.Add(id, addValue);
        }
        else
        {
            if(_collectionData[id] < maxValue)
            {
                _collectionData[id] += addValue;
            }
        }
    }

    public override uint GetCollection(uint id)
    {
        if (!_collectionData.ContainsKey(id))
        {
            return _collectionData[id];
        }
        return 0;
    }
}
