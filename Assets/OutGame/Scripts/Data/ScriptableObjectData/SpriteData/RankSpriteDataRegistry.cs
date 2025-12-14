using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteData;

[CreateAssetMenu(fileName = "RankData", menuName = "ScriptableObject/RankSpriteData")]
public class RankSpriteDataRegistry : DataRegistryBase<RankSprite>
{
    public Sprite GetData(RankType rankType)
    {
        foreach (var rank in _dataHolder)
        {
            if (rank.Rank == rankType)
            {
                return rank.Sprite;
            }
        }
        return null;
    }
}