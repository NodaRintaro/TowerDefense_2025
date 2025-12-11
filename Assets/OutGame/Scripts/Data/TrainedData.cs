using CharacterData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TrainedData
{
    [System.Serializable, CreateAssetMenu(fileName = "TrainedCharacterRegistry", menuName = "ScriptableObject/TrainedCharacterRegistry")]
    public class TrainedCharacterHolder : ScriptableObject
    {
        [SerializeField, Header("育成済みキャラクターのデータリスト")]
        private List<TrainedCharacterData> _trainedCharacterDataList = new();

        public List<TrainedCharacterData> TrainedCharacterDataList => _trainedCharacterDataList;
    }


    /// <summary> トレーニング済みのキャラクターデータ </summary>
    [Serializable]
    public class TrainedCharacterData : CharacterBaseData
    {
        [SerializeField, Header("トレーニング後のキャラクターID")]
        private string _trainiedID;

        [SerializeField, Header("トレーニング後のキャラクターランク")]
        private RankType _rankType;

        [SerializeField, Header("体力増加値")]
        private uint _addPhysical;
        [SerializeField, Header("筋力増加値")]
        private uint _addPower;
        [SerializeField, Header("知力増加値")]
        private uint _addIntelligence;
        [SerializeField, Header("素早さ増加値")]
        private uint _addSpeed;

        /// <summary> トレーニング後のキャラクターデータID </summary>
        public string TrainedCharacterID => _trainiedID;

        #region 増加値の参照用プロパティ
        public uint AddPhysical => _addPhysical;
        public uint AddPower => _addPower;
        public uint AddIntelligence => _addIntelligence;
        public uint AddSpeed => _addSpeed;
        #endregion

        #region 合計値の参照プロパティ
        public uint TotalPhysical => _addPhysical + _basePhysical;
        public uint TotalPower => _addPower + _basePower;
        public uint TotalIntelligence => _baseIntelligence;
        public uint TotalSpeed => _addSpeed + _baseSpeed;
        #endregion

        public void SetCharacterTrainedParameterData(string newID, uint setPhysi, uint setPow, uint setInt, uint setSp)
        {
            _trainiedID = newID;
            _addPhysical = setPhysi;
            _addPower = setPow;
            _addIntelligence = setInt;
            _addSpeed = setSp;
        }

        public void SetCharacterRank(RankType rankType)
        {
            _rankType = rankType;
        }
    }

}