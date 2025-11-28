using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 送られた数値によってランクを決定するクラス
/// </summary>
[CreateAssetMenu(fileName = "RankData", menuName = "ScriptableObject/RankSelecter")]
public class RankSelecter : ScriptableObject
{
    [SerializeField, Header("各ランクのデータ")]
    private RankData[] _rankDataArray;

    public RankType GetRank(uint rankSelectNum)
    {
        uint saveRankNum = 0;

        foreach (var rank in _rankDataArray)
        {
            if (saveRankNum <= rankSelectNum && rank.NextRankNum > rankSelectNum)
            {
                return rank.RankType;
            }
        }

        return RankType.None;
    }

    [Serializable]
    public struct RankData
    {
        [SerializeField, Header("Rankのタイプ")]
        public RankType RankType;

        [SerializeField, Header("次のランクに到達できる数値")]
        public uint NextRankNum;
    }
}