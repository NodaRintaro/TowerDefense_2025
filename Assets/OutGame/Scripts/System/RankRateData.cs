using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankRateData
{
    public static readonly Dictionary<RankType, uint> CharacterRankRate =
        new Dictionary<RankType, uint>
        {
                { RankType.F , 0 },
                { RankType.E , 300 },
                { RankType.D , 600 },
                { RankType.C , 900 },
                { RankType.B , 1200 },
                { RankType.A , 1500 },
                { RankType.S , 2000 },
                { RankType.SS , 2500 },
                { RankType.SSS , 3000 },
        };

    public static readonly Dictionary<RankType, uint> ParamRankRate =
        new Dictionary<RankType, uint>
        {
                { RankType.F , 0 },
                { RankType.E , 100 },
                { RankType.D , 200 },
                { RankType.C , 300 },
                { RankType.B , 400 },
                { RankType.A , 500 },
                { RankType.S , 600 },
                { RankType.SS , 800 },
                { RankType.SSS , 1000 },
        };
}
