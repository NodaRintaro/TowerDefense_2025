using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RankSelectSystem
{
    public class RankSelecter
    {
        private ParameterRankSelectMenu _parameterRankSelectMenu = null; 

        private CharacterRankSelectMenu _characterRankSelectMenu = null;

        /// <summary>
        /// 与えられた数字からランクを選別する関数
        /// </summary>
        public RankType GetCurrentRank(uint targetNum, SelectRankType selectType)
        {
            RankSelectMenuBase menu = GetSelectRankType(selectType);
            RankType currntRank = RankType.None;

            foreach(RankType value in Enum.GetValues(typeof(RankType)))
            {
                if (menu.RankSelectMenuDict[value] > targetNum)
                {
                    return currntRank;
                }

                currntRank = value;
            }

            return currntRank;
        }

        public RankSelectMenuBase GetSelectRankType(SelectRankType selectRank) 
        {
            switch(selectRank)
            {
                case SelectRankType.ParameterRank:
                    if(_parameterRankSelectMenu == null)
                    {
                        return _parameterRankSelectMenu = new();
                    }
                    return _parameterRankSelectMenu;
                case SelectRankType.CharacterRank:
                    if(_characterRankSelectMenu == null)
                    {
                        return _characterRankSelectMenu = new();
                    }
                    return _characterRankSelectMenu;
            }
            return null;
        }
    }

    /// <summary>
    /// ランクを選別する際の指標となるDictionaryを保管するクラス
    /// </summary>
    public abstract class RankSelectMenuBase
    {
        protected Dictionary<RankType, uint> _rankSelectMenuDict = null;

        public Dictionary<RankType, uint> RankSelectMenuDict => _rankSelectMenuDict;

        public RankSelectMenuBase()
        {
            SetDict();
        }

        public abstract void SetDict();
    }

    /// <summary>
    /// Parameterランクを決定する際の指標
    /// </summary>
    public class ParameterRankSelectMenu : RankSelectMenuBase
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

    public class CharacterRankSelectMenu : RankSelectMenuBase
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
}
