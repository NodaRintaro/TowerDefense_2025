using System.Collections.Generic;

public class CharacterRankRateData : RankRateDataBase
{
    public override void SetDict()
    {
        _rankSelectMenuDict =
        new Dictionary<RankType, uint>
        {
                {RankType.F , 0 },
                {RankType.E , 300 },
                {RankType.D , 600 },
                {RankType.C , 900 },
                {RankType.B , 1200 },
                {RankType.A , 1500 },
                {RankType.S , 2000 },
                {RankType.SS , 2500 },
                {RankType.SSS , 3000 },
        };
    }
}