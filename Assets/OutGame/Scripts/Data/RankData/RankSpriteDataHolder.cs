using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RankData", menuName = "RankData/SupportCardDataHolder")]
public class RankSpriteDataHolder : ScriptableObject
{
    [SerializeField, Header("Rankのデータリスト")]
    RankSpriteData[] _rankList;

    public RankSpriteData[] RankDataList => _rankList;
}