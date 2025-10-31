using System;
using UnityEngine;

[Serializable]
public class RankData
{
    [SerializeField, Header("RankのSpriteData")]
    private Sprite _rankSprite;

    [SerializeField, Header("ランクのタイプ")]
    private RankType _rankType;

    public Sprite RankSprite => _rankSprite;
    public RankType RankType => _rankType;
}
