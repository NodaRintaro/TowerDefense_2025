using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 渡されたRankの区分表を元にランクを決定するロジッククラス
/// </summary>
public class RankSelecter
{
    /// <summary>
    /// 与えられた数字からランクを選別する関数
    /// </summary>
    public RankType GetCurrentRank(uint targetNum, RankRateDataBase rateData)
    {
        RankType currntRank = RankType.None;

        foreach (RankType value in Enum.GetValues(typeof(RankType)))
        {
            if (rateData.RankSelectMenuDict[value] > targetNum)
            {
                return currntRank;
            }

            currntRank = value;
        }

        return currntRank;
    }
}

/// <summary>
/// ランクの区分表のデータクラスの基底クラス
/// </summary>
public abstract class RankRateDataBase
{
    protected Dictionary<RankType, uint> _rankSelectMenuDict = null;

    public Dictionary<RankType, uint> RankSelectMenuDict => _rankSelectMenuDict;

    public RankRateDataBase()
    {
        SetDict();
    }

    public abstract void SetDict();
}
