using System;
using UnityEngine;

[Serializable]
public class RankData
{
    [SerializeField, Header("RankのSpriteData")]
    private Sprite _rankSprite;

    [SerializeField, Header("ランクのタイプ")]
    private RankType _rankType;

    [SerializeField, Header("次のランクに到達するまでの値")]
    private uint _rankUpNum;

    /// <summary> ランクのスプライト </summary>
    public Sprite RankSprite => _rankSprite;
    /// <summary> ランクの種類 </summary>
    public RankType RankType => _rankType;
    /// <summary> 次のランクに到達するまでの値 </summary>
    public uint RankUpNum => _rankUpNum;
}
