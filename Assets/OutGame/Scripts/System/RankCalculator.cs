using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 渡されたRankの区分表を元にランクを決定するロジッククラス
/// </summary>
public class RankCalculator
{
    /// <summary>
    /// 与えられた数字からランクを選別する関数
    /// </summary>
    public static RankType GetCurrentRank(uint targetNum, Dictionary<RankType, uint> rateData)
    {
        RankType currntRank = RankType.None;

        foreach (RankType value in Enum.GetValues(typeof(RankType)))
        {
            if (rateData[value] > targetNum)
            {
                return currntRank;
            }

            currntRank = value;
        }

        return currntRank;
    }

    public static uint GetNextRankNum(uint targetNum, Dictionary<RankType, uint> rateData)
    {
        foreach (RankType value in Enum.GetValues(typeof(RankType)))
        {
            if (rateData[value] > targetNum)
            {
                return rateData[value];
            }
        }
        return 9999;
    }

    public static uint GetCurrentRankMinNum(uint targetNum, Dictionary<RankType, uint> rateData)
    {
        uint saveNum = 0;
        foreach (RankType value in Enum.GetValues(typeof(RankType)))
        {
            if (rateData[value] > targetNum)
            {
                return saveNum;
            }
            saveNum = rateData[value];
        }
        return 9999;
    }
}


