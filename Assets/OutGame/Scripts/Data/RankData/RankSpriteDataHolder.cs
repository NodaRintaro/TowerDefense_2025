using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RankData", menuName = "ScriptableObject/RankSpriteData")]
public class RankSpriteDataHolder : ScriptableObject
{
    [SerializeField, Header("RankのSpriteデータリスト")]
    RankSpriteData[] _rankSpriteDataList;

    public RankSpriteData[] RankSpriteDataList => _rankSpriteDataList;


    public Sprite GetRankSprite(RankType rankType)
    {
        foreach (var rank in _rankSpriteDataList)
        {
            if(rank.RankType == rankType)
            {
                return rank.RankSprite;
            }
        }
        return null;
    }
}