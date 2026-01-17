using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseCharacterRankRateData : IRankRate
{
    //パラメータのランクを決定する際の各ランクの分布図
    public static readonly Dictionary<RankType, uint> RankRateDict =
        new()
        {
            {RankType.F, 0 },
            {RankType.E, 300 },
            {RankType.D, 600 },
            {RankType.C, 900 },
            {RankType.B, 1200 },
            {RankType.A, 1500 },
            {RankType.S, 1800 },
            {RankType.SS, 2100 },
            {RankType.SSS, 2400 },
        };
}
