using System;
using UnityEngine;

[Serializable]
public class RankSpriteData
{
    [SerializeField, Header("RankのSpriteData")]
    private Sprite _rankSprite;

    [SerializeField, Header("ランクのタイプ")]
    private RankType _rankType;

    /// <summary> ランクのスプライト </summary>
    public Sprite RankSprite => _rankSprite;
    /// <summary> ランクの種類 </summary>
    public RankType RankType => _rankType;
}
