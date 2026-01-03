using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameterRankRateData : IRankRate
{
    //パラメータのランクを決定する際の各ランクの分布図
    public static readonly Dictionary<RankType, uint> RankRateDict =
        new()
        {
            {RankType.F, 0 },
            {RankType.E, 100 },
            {RankType.D, 200 },
            {RankType.C, 300 },
            {RankType.B, 400 },
            {RankType.A, 500 },
            {RankType.S, 600 },
            {RankType.SS, 800 },
            {RankType.SSS, 1000 },
        };
}
