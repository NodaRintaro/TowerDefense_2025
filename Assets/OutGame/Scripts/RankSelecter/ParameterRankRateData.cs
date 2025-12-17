using System.Collections.Generic;

/// <summary>
/// Parameterランクを決定する際の指標
/// </summary>
public class ParameterRankRateData : RankRateDataBase
{
    public override void SetDict()
    {
        _rankSelectMenuDict =
        new Dictionary<RankType, uint>
        {
                {RankType.F , 0 },
                {RankType.E , 100 },
                {RankType.D , 200 },
                {RankType.C , 300 },
                {RankType.B , 400 },
                {RankType.A , 500 },
                {RankType.S , 600 },
                {RankType.SS , 800 },
                {RankType.SSS , 1000 },
        };
    }
}