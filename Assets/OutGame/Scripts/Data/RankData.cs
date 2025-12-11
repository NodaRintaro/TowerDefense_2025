using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RankData
{
    /// <summary>
    /// 送られた数値によってランクを決定するクラス
    /// </summary>
    [CreateAssetMenu(fileName = "RankData", menuName = "ScriptableObject/RankSelecter")]
    public class RankSelectMenu : ScriptableObject
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
                if (rank.RankType == rankType)
                {
                    return rank.RankSprite;
                }
            }
            return null;
        }
    }

}